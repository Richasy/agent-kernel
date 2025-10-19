// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using global::Ollama;
using Microsoft.Extensions.AI;
using RichasyKernel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace Richasy.AgentKernel.Connectors.Ollama.Core;

/// <summary>
/// Ollama chat client.
/// </summary>
public sealed class OllamaChatClient : Microsoft.Extensions.AI.IChatClient
{
    private readonly OllamaApiClient _ollamaClient;
    private readonly string? _modelId;

    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaChatClient"/> class.
    /// </summary>
    /// <param name="endpoint">Ollama endpoint.</param>
    /// <param name="modelId">Model ID.</param>
    public OllamaChatClient(string endpoint, string? modelId)
    {
        ArgumentException.ThrowIfNullOrEmpty(endpoint, nameof(endpoint));
        _ollamaClient = new OllamaApiClient(baseUri: new Uri(endpoint));
        _modelId = modelId;
        Metadata = new("ollama", new Uri(endpoint), modelId);
    }

    /// <inheritdoc/>
    public ChatClientMetadata Metadata { get; }

    /// <inheritdoc/>
    public void Dispose()
        => _ollamaClient?.Dispose();

    /// <inheritdoc/>
    public object? GetService(Type serviceType, object? serviceKey = null)
    {
        ArgumentNullException.ThrowIfNull(serviceType);
        return serviceKey is null && serviceType.IsInstanceOfType(this) ? this : null;
    }

    /// <inheritdoc/>
    public async Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(messages);

        var ollamaMessages = messages.Select(ToOllamaMessage).ToList();
        var modelId = options?.ModelId ?? _modelId ?? throw new KernelException("Model ID is required");
        var request = new GenerateChatCompletionRequest
        {
            Model = modelId,
            Messages = ollamaMessages,
            Stream = false,
            Options = ToRequestOptions(options),
            Tools = options?.Tools is { Count: > 0 } ? [.. options.Tools.Select(ToOllamaTool).Where(t => t != null)!] : null,
        };

        GenerateChatCompletionResponse? response = null;
        await foreach (var chunk in _ollamaClient.Chat.GenerateChatCompletionAsync(request, cancellationToken).ConfigureAwait(false))
        {
            response = chunk;
        }

        if (response == null)
        {
            throw new KernelException("Empty response");
        }

        return new([FromOllamaMessage(response.Message)])
        {
            ResponseId = response.Model,
            ModelId = response.Model,
            CreatedAt = response.CreatedAt,
            FinishReason = ToFinishReason(response),
            Usage = ParseOllamaResponseUsage(response),
        };
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(messages);

        var ollamaMessages = messages.Select(ToOllamaMessage).ToList();
        var modelId = options?.ModelId ?? _modelId ?? throw new KernelException("Model ID is required");
        var request = new GenerateChatCompletionRequest
        {
            Model = modelId,
            Messages = ollamaMessages,
            Stream = true,
            Options = ToRequestOptions(options),
            Tools = options?.Tools is { Count: > 0 } ? [.. options.Tools.Select(ToOllamaTool).Where(t => t != null)!] : null,
        };

        var role = string.Empty;
        await foreach (var chunk in _ollamaClient.Chat.GenerateChatCompletionAsync(request, cancellationToken).WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            var update = new ChatResponseUpdate
            {
                CreatedAt = chunk.CreatedAt,
                FinishReason = ToFinishReason(chunk),
                ModelId = chunk.Model,
            };

            if (chunk.Message is { } message)
            {
                if (!string.IsNullOrEmpty(message.Role.ToValueString()))
                {
                    role = message.Role.ToValueString();
                }

                update.Role = new(role);

                if (message.ToolCalls is { Count: > 0 })
                {
                    foreach (var toolCall in message.ToolCalls)
                    {
                        if (toolCall.Function is { })
                        {
                            var content = ToFunctionCallContent(toolCall);
                            if (content is not null)
                            {
                                update.Contents.Add(content);
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(message.Content) || update.Contents.Count == 0)
                {
                    update.Contents.Insert(0, new TextContent(message.Content));
                }
            }

            if (ParseOllamaResponseUsage(chunk) is { } usage)
            {
                update.Contents.Add(new UsageContent(usage));
            }

            yield return update;
        }
    }

    private static global::Ollama.Message ToOllamaMessage(ChatMessage message)
    {
        var ollamaMessage = new global::Ollama.Message
        {
            Role = message.Role.Value switch
            {
                "system" => MessageRole.System,
                "user" => MessageRole.User,
                "assistant" => MessageRole.Assistant,
                "tool" => MessageRole.Tool,
                _ => MessageRole.User,
            },
            Content = string.Empty,
        };

        if (message.Contents?.Count > 0)
        {
            foreach (var content in message.Contents)
            {
                switch (content)
                {
                    case TextContent textContent:
                        ollamaMessage.Content = textContent.Text ?? string.Empty;
                        break;
                    case FunctionCallContent fcc:
                        ollamaMessage.Role = MessageRole.Assistant;
                        ollamaMessage.ToolCalls ??= [];
                        var sb = new StringBuilder();
                        DictionaryToolkit.WriteDictionary(sb, fcc.Arguments ?? new Dictionary<string, object?>());
                        ollamaMessage.ToolCalls.Add(new ToolCall
                        {
                            Function = new ToolCallFunction
                            {
                                Name = fcc.Name,
                                Arguments = JsonDocument.Parse(sb.ToString()).RootElement,
                            }
                        });
                        break;
                    case FunctionResultContent frc:
                        ollamaMessage.Role = MessageRole.Tool;
                        ollamaMessage.Content = frc.Result?.ToString() ?? string.Empty;
                        break;
                }
            }
        }

        return ollamaMessage;
    }

    private static ChatMessage FromOllamaMessage(global::Ollama.Message message)
    {
        List<AIContent> contents = [];
        var role = message.Role.ToValueString();

        if (message.ToolCalls is { Count: > 0 })
        {
            role = "assistant";
            foreach (var toolCall in message.ToolCalls)
            {
                if (toolCall.Function is { })
                {
                    var content = ToFunctionCallContent(toolCall);
                    if (content is not null)
                    {
                        contents.Add(content);
                    }
                }
            }
        }

        if (!string.IsNullOrEmpty(message.Content))
        {
            contents.Insert(0, new TextContent(message.Content));
        }

        return new ChatMessage(new(role), contents);
    }

    private static ChatFinishReason? ToFinishReason(GenerateChatCompletionResponse response) =>
        response.Done ? new ChatFinishReason(response.DoneReason?.ToString() ?? "stop") : null;

    private static global::Ollama.Tool? ToOllamaTool(AITool tool)
    {
        if (tool is AIFunction function)
        {
            return new global::Ollama.Tool
            {
                Type = ToolType.Function,
                Function = new ToolFunction
                {
                    Name = function.Name,
                    Description = function.Description ?? string.Empty,
                    Parameters = function.JsonSchema,
                },
            };
        }

        return null;
    }

    private static FunctionCallContent? ToFunctionCallContent(ToolCall call)
    {
        if (call.Function is not { } func)
        {
            return null;
        }

        var callContent = new FunctionCallContent(func.Name, func.Name);

        if (func.Arguments is JsonElement jsonElement)
        {
            callContent.Arguments = JsonToolkit.JsonElementToDictionary(jsonElement);
        }

        return callContent;
    }

    private static UsageDetails? ParseOllamaResponseUsage(GenerateChatCompletionResponse response)
    {
        if (response.PromptEvalCount == null && response.EvalCount == null)
        {
            return null;
        }

        return new UsageDetails
        {
            InputTokenCount = response.PromptEvalCount,
            OutputTokenCount = response.EvalCount,
            TotalTokenCount = (response.PromptEvalCount ?? 0) + (response.EvalCount ?? 0),
        };
    }

    private static RequestOptions? ToRequestOptions(ChatOptions? options)
    {
        if (options == null)
        {
            return null;
        }

        return new RequestOptions
        {
            Temperature = options.Temperature,
            TopP = options.TopP,
            FrequencyPenalty = options.FrequencyPenalty,
            PresencePenalty = options.PresencePenalty,
            Stop = options.StopSequences?.ToList(),
            NumPredict = options.MaxOutputTokens,
        };
    }
}
