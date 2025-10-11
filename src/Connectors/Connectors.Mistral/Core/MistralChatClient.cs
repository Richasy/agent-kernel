// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using Richasy.AgentKernel.Connectors.Mistral.Models;
using RichasyKernel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace Richasy.AgentKernel.Connectors.Mistral.Core;

/// <summary>
/// Mistral chat client.
/// </summary>
public sealed class MistralChatClient : IChatClient
{
    private readonly string _apiEndpoint;
    private readonly HttpClient _httpClient;
    private readonly string _accessKey;

    /// <summary>
    /// Initializes a new instance of the <see cref="MistralChatClient"/> class.
    /// </summary>
    public MistralChatClient(string accessKey, bool useCodestralUri, string? modelId)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessKey, nameof(accessKey));
        _accessKey = accessKey;
        _apiEndpoint = useCodestralUri ? "https://codestral.mistral.ai/v1/chat/completions" : "https://api.mistral.ai/v1/chat/completions";
        _httpClient = HttpExtensions.CreateHttpClient();
        Metadata = new("mistral", new(_apiEndpoint), modelId);
    }

    /// <inheritdoc/>
    public ChatClientMetadata Metadata { get; }

    /// <inheritdoc/>
    public async Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(messages);
        var chatRequest = ToMistralChatRequest(messages, options, stream: false);
        var json = JsonSerializer.Serialize(chatRequest, JsonGenContext.Default.MistralChatRequest);
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, _apiEndpoint);
        httpRequest.Headers.Add("Authorization", $"Bearer {_accessKey}");
        httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");
        using var response = await _httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        var responseJson = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode && responseJson.Contains("\"detail\":", StringComparison.InvariantCultureIgnoreCase))
        {
            var error = JsonSerializer.Deserialize(responseJson, JsonGenContext.Default.MistralErrorResponse);
            throw new KernelException(error?.Detail?.FirstOrDefault()?.Msg ?? "Unknown error");
        }

        response.EnsureSuccessStatusCode();
        var responseObj = JsonSerializer.Deserialize(responseJson, JsonGenContext.Default.MistralChatResponse);
        return new([FromMistralChoice(responseObj?.Choices?.First() ?? throw new KernelException("Empty response"))])
        {
            ResponseId = responseObj.Id,
            ModelId = responseObj?.Model ?? options?.ModelId,
            CreatedAt = DateTimeOffset.FromUnixTimeSeconds(responseObj?.Created ?? 0),
            FinishReason = ToFinishReason(responseObj!),
            Usage = ParseMistralChatResponseUsage(responseObj!),
        };
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(messages);
        var chatRequest = ToMistralChatRequest(messages, options, stream: true);
        var json = JsonSerializer.Serialize(chatRequest, JsonGenContext.Default.MistralChatRequest);
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, _apiEndpoint);
        httpRequest.Headers.Add("Authorization", $"Bearer {_accessKey}");
        httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");
        using var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
        using var httpResponseStream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        using var streamReader = new StreamReader(httpResponseStream);
        var role = string.Empty;
        while ((await streamReader.ReadLineAsync(cancellationToken).ConfigureAwait(false)) is { } line)
        {
            if (line.StartsWith("\"detail\":", StringComparison.InvariantCultureIgnoreCase))
            {
                var errorResponse = JsonSerializer.Deserialize(line, JsonGenContext.Default.MistralErrorResponse);
                throw new KernelException(errorResponse?.Detail?.FirstOrDefault()?.Msg ?? "Unknown error");
            }

            if (line.StartsWith("data:", StringComparison.InvariantCultureIgnoreCase))
            {
                line = line[5..].Trim();
                if (line == "[Done]" || !line.StartsWith('{'))
                {
                    break;
                }

                var chunk = JsonSerializer.Deserialize(line, JsonGenContext.Default.MistralChatResponse);
                if (chunk == null)
                {
                    continue;
                }

                var modelId = chunk.Model ?? options?.ModelId;
                var update = new ChatResponseUpdate
                {
                    CreatedAt = DateTimeOffset.FromUnixTimeSeconds(chunk.Created),
                    FinishReason = ToFinishReason(chunk),
                    ModelId = modelId,
                };

                if (chunk.Choices?.FirstOrDefault()?.Delta is { } message)
                {
                    if (!string.IsNullOrEmpty(message.Role))
                    {
                        role = message.Role;
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

                    if (message.Content?.Length > 0 || update.Contents.Count == 0)
                    {
                        update.Contents.Insert(0, new TextContent(message.Content));
                    }
                }

                if (ParseMistralChatResponseUsage(chunk) is { } usage)
                {
                    update.Contents.Add(new UsageContent(usage));
                }

                yield return update;
            }
        }
    }

    /// <inheritdoc/>
    public void Dispose()
        => _httpClient?.Dispose();

    /// <inheritdoc/>
    public object? GetService(Type serviceType, object? serviceKey = null)
    {
        ArgumentNullException.ThrowIfNull(serviceType);
        return serviceKey is null && serviceType.IsInstanceOfType(this) ? this : null;
    }

    private static MistralChatRequest ToMistralChatRequest(IEnumerable<ChatMessage> chatMessages, ChatOptions? options, bool stream)
    {
        var request = new MistralChatRequest
        {
            Messages = [.. chatMessages.Select(ToMistralChatMessage)],
            Model = options?.ModelId?? string.Empty,
            Stream = stream,
        };

        if (options is not null)
        {
            request.MaxTokens = options.MaxOutputTokens;
            request.Temperature = options.Temperature;
            request.TopP = options.TopP;
            request.PresencePenalty = options.PresencePenalty;
            request.FrequencyPenalty = options.FrequencyPenalty;
            request.Stop = options.StopSequences?.ToArray();
            request.ResponseFormat = options.ResponseFormat is not null
                ? options.ResponseFormat is ChatResponseFormatJson ? MistralResponseFormat.JsonFormat : MistralResponseFormat.TextFormat
                : null;
        }

        if (options?.Tools is { Count: > 0 } tools)
        {
            var actualTools = tools.Select(ToMistralTool).Where(p => p != null).Select(p => p!);
            request.Tools = actualTools?.Count() > 0 ? [.. actualTools] : null;
        }

        return request;
    }

    private static MistralChatMessage ToMistralChatMessage(ChatMessage message)
    {
        if (message.Contents?.Count > 0)
        {
            switch (message.Contents[0])
            {
                case TextContent textContent:
                    return new MistralChatMessage { Role = message.Role.Value, Content = textContent.Text };
                case FunctionCallContent fcc:
                    var sb = new StringBuilder();
                    DictionaryToolkit.WriteDictionary(sb, fcc.Arguments ?? new Dictionary<string, object?>());
                    return new MistralChatMessage
                    {
                        Role = "assistant",
                        Content = string.Empty,
                        ToolCalls = [new MistralToolCall
                        {
                            Id = fcc.CallId,
                            Type = "function",
                            Function = new MistralFunctionCall{
                                Name = fcc.Name,
                                Arguments = BinaryData.FromString(sb.ToString())
                            }
                        }],
                    };
                case FunctionResultContent frc:
                    return new MistralChatMessage
                    {
                        ToolCallId = frc.CallId,
                        Name = frc.CallId,
                        Content = frc.Result?.ToString() ?? string.Empty,
                        Role = "tool",
                    };
                default:
                    break;
            }

            return new MistralChatMessage { Role = message.Role.Value, Content = message.Text ?? string.Empty };
        }

        throw new NotSupportedException();
    }

    private static ChatMessage FromMistralChoice(MistralChatResponseChoice choice)
    {
        List<AIContent> contents = [];
        var role = string.Empty;

        if (choice.Message?.ToolCalls is { Count: > 0 })
        {
            role = "assistant";
            foreach (var toolCall in choice.Message.ToolCalls)
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

        if (!string.IsNullOrEmpty(choice.Message?.Content))
        {
            contents.Insert(0, new TextContent(choice.Message.Content));
            role = choice.Message.Role;
        }

        return new ChatMessage(new(role!), contents);
    }

    private static ChatFinishReason? ToFinishReason(MistralChatResponse response) =>
        response.Choices?.First().FinishReason switch
        {
            null => null,
            _ => new(response.Choices?.First().FinishReason!),
        };

    private static MistralTool? ToMistralTool(AITool tool)
    {
        if (tool is AIFunction function)
        {
            return new()
            {
                Type = "function",
                Function = new()
                {
                    Name = function.Name,
                    Description = function.Description,
                    Parameters = function.JsonSchema.Deserialize(JsonGenContext.Default.MistralFunctionToolParameters),
                },
            };
        }

        return default;
    }

    private static FunctionCallContent? ToFunctionCallContent(MistralToolCall call)
    {
        var callContent = new FunctionCallContent(call.Id ?? call.Function.Name, call.Function.Name ?? string.Empty);
        var argumentJson = call.Function.Arguments?.ToString() ?? "\"{\"}";
        var unescapedJson = JsonSerializer.Deserialize(argumentJson, JsonGenContext.Default.String);
        callContent.Arguments = JsonToolkit.JsonElementToDictionary(JsonDocument.Parse(unescapedJson!).RootElement);
        return callContent;
    }

    private static UsageDetails? ParseMistralChatResponseUsage(MistralChatResponse response)
    {
        return response.Usage is null
            ? null
            : new UsageDetails
            {
                InputTokenCount = response.Usage.PromptTokens,
                OutputTokenCount = response.Usage.CompletionTokens,
                TotalTokenCount = response.Usage.TotalTokens,
            };
    }
}
