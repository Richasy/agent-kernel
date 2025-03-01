// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using Microsoft.ML.OnnxRuntimeGenAI;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace Richasy.AgentKernel.Connectors.Onnx.Core;

/// <summary>
/// Onnx Chat Client.
/// </summary>
public sealed class OnnxChatClient : IChatClient
{
    private const string TEMPLATE_PLACEHOLDER = "{CONTENT}";
    private const int DefaultTopK = 50;
    private const float DefaultTopP = 0.9f;
    private const float DefaultTemperature = 1;
    private const int DefaultMinLength = 0;
    private const int DefaultMaxLength = 1024;
    private const bool DefaultDoSample = false;

    private static readonly SemaphoreSlim _createSemaphore = new(1, 1);
    private static OgaHandle? _ogaHandle;
    private Model? _model;
    private Tokenizer? _tokenizer;

    /// <summary>
    /// Initializes a new instance of the <see cref="OnnxChatClient"/> class.
    /// </summary>
    public OnnxChatClient(string? modelDir)
    {
        Metadata = new("onnx", default, modelDir);
    }

    /// <inheritdoc/>
    public ChatClientMetadata Metadata { get; }

    /// <inheritdoc/>
    public async Task<ChatResponse> GetResponseAsync(IList<ChatMessage> chatMessages, ChatOptions? options = null, CancellationToken cancellationToken = default)
    {
        var responseText = new StringBuilder();
        await foreach (var completion in GetStreamingResponseAsync(chatMessages, options, cancellationToken).ConfigureAwait(false))
        {
            responseText.Append(completion.Text);
        }

        return new(new ChatMessage(ChatRole.Assistant, responseText.ToString()));
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IList<ChatMessage> chatMessages, ChatOptions? options = null, [EnumeratorCancellation]CancellationToken cancellationToken = default)
    {
        var systemTemplate = options?.AdditionalProperties?.GetValueOrDefault("system_template") as string;
        var userTemplate = options?.AdditionalProperties?.GetValueOrDefault("user_template") as string;
        var assistantTemplate = options?.AdditionalProperties?.GetValueOrDefault("assistant_template") as string;
        var promptTemplate = options?.AdditionalProperties?.GetValueOrDefault("prompt_template") as string;
        var stops = options?.StopSequences?.ToArray();
        var modelId = options?.ModelId ?? Metadata.ModelId;
        if (string.IsNullOrEmpty(systemTemplate)
            && string.IsNullOrEmpty(userTemplate)
            && string.IsNullOrEmpty(assistantTemplate))
        {
            var inferenceModelFile = Path.Combine(modelId!, "inference_model.json");
            if (File.Exists(inferenceModelFile))
            {
                var content = await File.ReadAllTextAsync(inferenceModelFile, cancellationToken).ConfigureAwait(false);
                var inferenceModel = JsonSerializer.Deserialize(content, JsonGenContext.Default.InferenceModel);
                systemTemplate = inferenceModel?.PromptTemplate?.system;
                userTemplate = inferenceModel?.PromptTemplate?.user;
                assistantTemplate = inferenceModel?.PromptTemplate?.assistant;
                promptTemplate = inferenceModel?.PromptTemplate?.prompt;
            }
        }
        
        await InitializeAsync(modelId!, cancellationToken).ConfigureAwait(false);
        var prompt = GetPrompt(chatMessages, systemTemplate, userTemplate, assistantTemplate, promptTemplate, stops);

        await Task.CompletedTask.ConfigureAwait(ConfigureAwaitOptions.ForceYielding);

        using var generatorParams = new GeneratorParams(_model);

        using var sequences = _tokenizer!.Encode(prompt);

        void TransferMetadataValue(string propertyName, object defaultValue)
        {
            object? val = null;
            options?.AdditionalProperties?.TryGetValue(propertyName, out val);

            val ??= defaultValue;

            if (val is int intVal)
            {
                generatorParams.SetSearchOption(propertyName, intVal);
            }
            else if (val is float floatVal)
            {
                generatorParams.SetSearchOption(propertyName, floatVal);
            }
            else if (val is bool boolVal)
            {
                generatorParams.SetSearchOption(propertyName, boolVal);
            }
        }

        if (options != null)
        {
            TransferMetadataValue("min_length", DefaultMinLength);
            TransferMetadataValue("do_sample", DefaultDoSample);
            generatorParams.SetSearchOption("temperature", (double)(options.Temperature ?? DefaultTemperature));
            generatorParams.SetSearchOption("top_p", (double)(options.TopP ?? DefaultTopP));
            generatorParams.SetSearchOption("top_k", options.TopK ?? DefaultTopK);
        }

        generatorParams.SetSearchOption("max_length", (options?.MaxOutputTokens ?? DefaultMaxLength) + sequences[0].Length);
        generatorParams.TryGraphCaptureWithMaxBatchSize(1);

        using var tokenizerStream = _tokenizer.CreateStream();
        using var generator = new Generator(_model, generatorParams);
        generator.AppendTokenSequences(sequences);
        StringBuilder stringBuilder = new();
        var stopTokensAvailable = stops != null && stops.Length > 0;
        while (!generator.IsDone())
        {
            string part;
#pragma warning disable CA1031 // 不捕获常规异常类型
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                generator.GenerateNextToken();
                part = tokenizerStream.Decode(generator.GetSequence(0)[^1]);

                if (cancellationToken.IsCancellationRequested && stopTokensAvailable)
                {
                    part = stops!.Last();
                }

                stringBuilder.Append(part);

                if (stopTokensAvailable)
                {
                    var str = stringBuilder.ToString();
                    if (stops!.Any(str.Contains))
                    {
                        break;
                    }
                }
            }
            catch (Exception)
            {
                break;
            }
#pragma warning restore CA1031 // 不捕获常规异常类型

            yield return new()
            {
                Role = ChatRole.Assistant,
                Text = part,
            };
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _model?.Dispose();
        _tokenizer?.Dispose();
        _ogaHandle?.Dispose();
    }

    /// <inheritdoc/>
    public object? GetService(Type serviceType, object? serviceKey = null)
    {
        ArgumentNullException.ThrowIfNull(serviceType);
        return serviceKey is null && serviceType.IsInstanceOfType(this) ? this : null;
    }

    private async Task InitializeAsync(string modelDir, CancellationToken cancellationToken)
    {
        var lockAcquired = false;

#pragma warning disable CA1031 // 不捕获常规异常类型
        try
        {
            await _createSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
            lockAcquired = true;
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Run(() =>
            {
                _model = new Model(modelDir);
                cancellationToken.ThrowIfCancellationRequested();
                _tokenizer = new Tokenizer(_model);
            }, cancellationToken).ConfigureAwait(false);
            _ogaHandle = new OgaHandle();
        }
        catch (Exception)
        {
            Dispose();
        }
        finally
        {
            if (lockAcquired)
            {
                _createSemaphore.Release();
            }
        }
#pragma warning restore CA1031 // 不捕获常规异常类型
    }

    private static string GetPrompt(
        IList<ChatMessage> history,
        string? systemTemplate,
        string? userTemplate,
        string? assistantTemplate,
        string? promptTemplate,
        string[]? stop)
    {
        if (!history.Any())
        {
            return string.Empty;
        }

        if (string.IsNullOrEmpty(systemTemplate)
            && string.IsNullOrEmpty(userTemplate)
            && string.IsNullOrEmpty(assistantTemplate)
            && string.IsNullOrEmpty(promptTemplate)
            && (stop is null || stop.Length == 0))
        {
            return string.Join(". ", history.Select(item => item.Text));
        }

        var prompt = new StringBuilder();
        var systemMsgWithoutSystemTemplate = string.Empty;

        for (var i = 0; i < history.Count - 1; i++)
        {
            var message = history[i];
            if (message.Role == ChatRole.System)
            {
                // ignore system prompts that aren't at the beginning
                if (i == 0)
                {
                    if (string.IsNullOrWhiteSpace(systemTemplate))
                    {
                        systemMsgWithoutSystemTemplate = message.Text ?? string.Empty;
                    }
                    else
                    {
                        prompt.Append(systemTemplate.Replace(TEMPLATE_PLACEHOLDER, message.Text, StringComparison.OrdinalIgnoreCase));
                    }
                }
            }
            else if (message.Role == ChatRole.User)
            {
                var msgText = message.Text ?? string.Empty;
                if (i == 1 && !string.IsNullOrWhiteSpace(systemMsgWithoutSystemTemplate))
                {
                    msgText = $"{systemMsgWithoutSystemTemplate} {msgText}";
                }

                prompt.Append(string.IsNullOrWhiteSpace(userTemplate) ?
                    msgText :
                    userTemplate.Replace(TEMPLATE_PLACEHOLDER, msgText, StringComparison.OrdinalIgnoreCase));
            }
            else if (message.Role == ChatRole.Assistant)
            {
                prompt.Append(string.IsNullOrWhiteSpace(assistantTemplate) ?
                    message.Text :
                    assistantTemplate.Replace(TEMPLATE_PLACEHOLDER, message.Text, StringComparison.OrdinalIgnoreCase));
            }
        }

        var lastMessage = history[^1]?.Text ?? string.Empty;
        if (string.IsNullOrEmpty(promptTemplate))
        {
            var userMsg = string.IsNullOrWhiteSpace(userTemplate) ?
                lastMessage :
                userTemplate.Replace(TEMPLATE_PLACEHOLDER, lastMessage, StringComparison.OrdinalIgnoreCase);
            if (!string.IsNullOrWhiteSpace(assistantTemplate))
            {
                var substringIndex = assistantTemplate.IndexOf(TEMPLATE_PLACEHOLDER, StringComparison.InvariantCulture);
                userMsg += assistantTemplate[..substringIndex];
            }

            prompt.Append(userMsg);
        }
        else
        {
            prompt.Append(promptTemplate.Replace(TEMPLATE_PLACEHOLDER, lastMessage, StringComparison.OrdinalIgnoreCase));
        }

        return prompt.ToString();
    }
}
