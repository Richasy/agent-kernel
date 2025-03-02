// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using Microsoft.ML.OnnxRuntimeGenAI;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

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
    private readonly bool _useCuda;
    private Config? _config;
    private Model? _model;
    private Tokenizer? _tokenizer;
    private string? _defaultSystemTemplate;
    private string? _defaultUserTemplate;
    private string? _defaultAssistantTemplate;
    private string? _defaultPromptTemplate;

#if USE_CUDA
    /// <summary>
    /// Initializes a new instance of the <see cref="OnnxChatClient"/> class.
    /// </summary>
    public OnnxChatClient(string? modelDir, bool useCuda)
    {
        _useCuda = useCuda;
        Metadata = new("onnx", default, modelDir);
    }
#else
    /// <summary>
    /// Initializes a new instance of the <see cref="OnnxChatClient"/> class.
    /// </summary>
    public OnnxChatClient(string? modelDir)
    {
        _useCuda = false;
        Metadata = new("onnx", default, modelDir);
    }
#endif

    /// <inheritdoc/>
    public ChatClientMetadata Metadata { get; }

    /// <inheritdoc/>
    public Task<ChatResponse> GetResponseAsync(IList<ChatMessage> chatMessages, ChatOptions? options = null, CancellationToken cancellationToken = default)
        => GetStreamingResponseAsync(chatMessages, options, cancellationToken).ToChatResponseAsync(cancellationToken: cancellationToken);

    /// <inheritdoc/>
    public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IList<ChatMessage> chatMessages, ChatOptions? options = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var modelId = options?.ModelId ?? Metadata.ModelId;
        if (_defaultSystemTemplate == null)
        {
            await LoadDefaultModelTemplateAsync(modelId!).ConfigureAwait(false);
        }

        var systemTemplate = GetValueFromChatOptions("system_template", options, _defaultSystemTemplate, JsonGenContext.Default.String);
        var userTemplate = GetValueFromChatOptions("user_template", options, _defaultUserTemplate, JsonGenContext.Default.String);
        var assistantTemplate = GetValueFromChatOptions("assistant_template", options, _defaultAssistantTemplate, JsonGenContext.Default.String);
        var promptTemplate = GetValueFromChatOptions("prompt_template", options, _defaultPromptTemplate, JsonGenContext.Default.String);
        var stops = options?.StopSequences?.ToArray();
        await InitializeAsync(modelId!, cancellationToken).ConfigureAwait(false);
        var prompt = GetPrompt(chatMessages, systemTemplate, userTemplate, assistantTemplate, promptTemplate, stops);

        await Task.CompletedTask.ConfigureAwait(ConfigureAwaitOptions.ForceYielding);

        using var generatorParams = new GeneratorParams(_model);

        using var sequences = _tokenizer!.Encode(prompt);

        void TransferMetadataValue<T>(string propertyName, T defaultValue, JsonTypeInfo<T> typeInfo)
        {
            var val = GetValueFromChatOptions(propertyName, options, defaultValue, typeInfo);
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
            TransferMetadataValue("min_length", DefaultMinLength, JsonGenContext.Default.Int32);
            TransferMetadataValue("do_sample", DefaultDoSample, JsonGenContext.Default.Boolean);
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
        _config?.Dispose();
        _model?.Dispose();
        _tokenizer?.Dispose();
        _ogaHandle?.Dispose();

        _config = null;
        _model = null;
        _tokenizer = null;
        _ogaHandle = null;
    }

    /// <inheritdoc/>
    public object? GetService(Type serviceType, object? serviceKey = null)
    {
        ArgumentNullException.ThrowIfNull(serviceType);
        return serviceKey is null && serviceType.IsInstanceOfType(this) ? this : null;
    }

    private async Task LoadDefaultModelTemplateAsync(string modelFolder)
    {
        var inferenceModelFile = Path.Combine(modelFolder, "model_template.json");
        if (File.Exists(inferenceModelFile))
        {
            var content = await File.ReadAllTextAsync(inferenceModelFile).ConfigureAwait(false);
            var inferenceModel = JsonSerializer.Deserialize(content, JsonGenContext.Default.ModelTemplate);
            _defaultSystemTemplate = inferenceModel?.system;
            _defaultUserTemplate = inferenceModel?.user;
            _defaultAssistantTemplate = inferenceModel?.assistant;
            _defaultPromptTemplate = inferenceModel?.prompt;
            return;
        }
        else
        {
            var genAIConfig = Path.Combine(modelFolder, "genai_config.json");
            if (File.Exists(genAIConfig))
            {
                var content = await File.ReadAllTextAsync(genAIConfig).ConfigureAwait(false);
                var genAIConfigModel = JsonDocument.Parse(content);
                if (genAIConfigModel?.RootElement.TryGetProperty("model", out var modelProp) == true)
                {
                    if (modelProp.TryGetProperty("type", out var typeProp) == true)
                    {
                        var modelType = typeProp.GetString() ?? string.Empty;
                        if (modelType.StartsWith("phi2", StringComparison.OrdinalIgnoreCase) || modelType.StartsWith("phi3", StringComparison.OrdinalIgnoreCase))
                        {
                            _defaultSystemTemplate = "<|system|>\n{Content}<|end>\n";
                            _defaultUserTemplate = "<|user|>\n{Content}<|end>\n";
                            _defaultAssistantTemplate = "<|assistant|>\n{Content}<|end>\n";
                            _defaultPromptTemplate = "<|user|>\n{Content}<|end>\n<|assistant|>\n";
                            return;
                        }
                        else if (modelType.StartsWith("phi4", StringComparison.OrdinalIgnoreCase))
                        {
                            _defaultSystemTemplate = "<|im_start|>system<|im_sep|>\n{Content}<|im_end|>\n";
                            _defaultUserTemplate = "<|im_start|>user<|im_sep|>\n{Content}<|im_end|>\n";
                            _defaultAssistantTemplate = "<|im_start|>assistant<|im_sep|>\n{Content}<|im_end|>\n";
                            _defaultPromptTemplate = "<|im_start|>user<|im_sep|>\n{Content}<|im_end|>\n<|im_start|>assistant<|im_sep|>\n";
                            return;
                        }
                        else if (modelType.StartsWith("llama", StringComparison.OrdinalIgnoreCase))
                        {
                            _defaultSystemTemplate = "<|begin_of_text|><|start_header_id|>system<|end_header_id|>\n{Content}<|eot_id|>\n";
                            _defaultUserTemplate = "<|start_header_id|>user<|end_header_id|>\n{Content}<|eot_id|>\n";
                            _defaultAssistantTemplate = "<|start_header_id|>assistant<|end_header_id|>\n{Content}<|eot_id|>\n";
                            _defaultPromptTemplate = "<|start_header_id|>user<|end_header_id|>\n{Content}<|eot_id|>\n<|start_header_id|>assistant<|end_header_id|>\n";
                            return;
                        }
                        else if (modelType.StartsWith("qwen2", StringComparison.OrdinalIgnoreCase))
                        {
                            _defaultSystemTemplate = "<|im_start|>system\n{Content}<|im_end|>\n\n";
                            _defaultUserTemplate = "<|im_start|>user\n{Content}<|im_end|>\n\n";
                            _defaultAssistantTemplate = "<|im_start|>assistant\n{Content}<|im_end|>\n\n";
                            _defaultPromptTemplate = "<|im_start|>user\n{Content}<|im_end|>\n<|im_start|>assistant\n\n";
                            return;
                        }
                    }
                }
            }
        }

        _defaultSystemTemplate = string.Empty;
        _defaultUserTemplate = string.Empty;
        _defaultAssistantTemplate = string.Empty;
        _defaultPromptTemplate = string.Empty;
    }

    private static T? GetValueFromChatOptions<T>(string key, ChatOptions? options, T defaultValue, JsonTypeInfo<T> typeInfo)
    {
        if (options?.AdditionalProperties?.Count > 0)
        {
            if (options?.AdditionalProperties?.GetValueOrDefault(key) is BinaryData data)
            {
                return JsonSerializer.Deserialize(data.ToString(), typeInfo);
            }

            return options?.AdditionalProperties?.GetValueOrDefault(key) is T value ? value : defaultValue;
        }

        return defaultValue;
    }

    private async Task InitializeAsync(string modelDir, CancellationToken cancellationToken)
    {
        if (_model != null)
        {
            return;
        }

        var lockAcquired = false;

#pragma warning disable CA1031 // 不捕获常规异常类型
        try
        {
            await _createSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
            lockAcquired = true;
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Run(() =>
            {
                _config = new Config(modelDir);
                if (_useCuda)
                {
                    _config.AppendProvider("cuda");
                    _config.SetProviderOption("cuda", "enable_cuda_graph", "0");
                }

                _model = new Model(_config);
                cancellationToken.ThrowIfCancellationRequested();
                _tokenizer = new Tokenizer(_model);
            }, cancellationToken).ConfigureAwait(false);
            _ogaHandle = new OgaHandle();
        }
        catch (Exception)
        {
            Dispose();
            throw;
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
            return string.Join("\n", history.Select(item => item.Text));
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
