// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using Richasy.AgentKernel.Connectors.Baidu.Models;
using RichasyKernel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace Richasy.AgentKernel.Connectors.Baidu.Core;

/// <summary>
/// 文心一言聊天客户端.
/// </summary>
public sealed class ErnieChatClient : IChatClient
{
    private const string _apiEndpoint = "https://qianfan.baidubce.com/v2/chat/completions";
    private readonly HttpClient _httpClient;
    private readonly ErnieServiceConfig _config;
    private BearerToken? _token;

    /// <summary>
    /// Initializes a new instance of the <see cref="ErnieChatClient"/> class.
    /// </summary>
    public ErnieChatClient(ErnieServiceConfig config)
    {
        ArgumentException.ThrowIfNullOrEmpty(config.AccessKey, nameof(config.AccessKey));
        ArgumentException.ThrowIfNullOrEmpty(config.SecretKey, nameof(config.SecretKey));

        _config = config;
        _httpClient = HttpExtensions.CreateHttpClient();
        Metadata = new("ernie", new Uri(_apiEndpoint), config.Model);
    }

    /// <inheritdoc/>
    public ChatClientMetadata Metadata { get; }

    /// <inheritdoc/>
    public async Task<ChatResponse> GetResponseAsync(IList<ChatMessage> chatMessages, ChatOptions? options = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(chatMessages);
        await CheckBearerTokenAsync(cancellationToken).ConfigureAwait(false);
        var chatRequest = ToErnieChatRequest(chatMessages, options, stream: false);
        var json = JsonSerializer.Serialize(chatRequest, JsonGenContext.Default.ErnieChatRequest);
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, _apiEndpoint);
        httpRequest.Headers.Add("Authorization", $"Bearer {_token!.Token}");
        httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");
        using var response = await _httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        var responseJson = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode || responseJson.Contains("\"error\":", StringComparison.InvariantCultureIgnoreCase))
        {
            var error = JsonSerializer.Deserialize(responseJson, JsonGenContext.Default.ErnieErrorResponse);
            throw new KernelException(error?.Error?.Message ?? "Unknown error");
        }

        var responseObj = JsonSerializer.Deserialize(responseJson, JsonGenContext.Default.ErnieChatResponse);
        return new([FromErnieChoice(responseObj?.Choices?.First() ?? throw new KernelException("Empty response"))])
        {
            ResponseId = responseObj.Id,
            ModelId = responseObj?.Model ?? options?.ModelId ?? Metadata.ModelId,
            CreatedAt = DateTimeOffset.FromUnixTimeSeconds(responseObj?.Created ?? 0),
            FinishReason = ToFinishReason(responseObj!),
            Usage = ParseErnieChatResponseUsage(responseObj!),
        };
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IList<ChatMessage> chatMessages, ChatOptions? options = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(chatMessages);
        await CheckBearerTokenAsync(cancellationToken).ConfigureAwait(false);
        var chatRequest = ToErnieChatRequest(chatMessages, options, stream: true);
        var json = JsonSerializer.Serialize(chatRequest, JsonGenContext.Default.ErnieChatRequest);
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, _apiEndpoint);
        httpRequest.Headers.Add("Authorization", $"Bearer {_token!.Token}");
        httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");
        using var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
        using var httpResponseStream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        using var streamReader = new StreamReader(httpResponseStream);
        while ((await streamReader.ReadLineAsync(cancellationToken).ConfigureAwait(false)) is { } line)
        {
            if (line.StartsWith("\"error\":", StringComparison.InvariantCultureIgnoreCase))
            {
                var errorResponse = JsonSerializer.Deserialize(line, JsonGenContext.Default.ErnieErrorResponse);
                throw new KernelException(errorResponse?.Error?.Message ?? "Unknown error");
            }

            if (line.StartsWith("data:", StringComparison.InvariantCultureIgnoreCase))
            {
                line = line[5..].Trim();
                if (line == "[Done]" || !line.StartsWith('{'))
                {
                    break;
                }

                var chunk = JsonSerializer.Deserialize(line, JsonGenContext.Default.ErnieChatResponse);
                if (chunk == null)
                {
                    continue;
                }

                var modelId = chunk.Model ?? options?.ModelId ?? Metadata.ModelId;
                var update = new ChatResponseUpdate
                {
                    Role = string.IsNullOrEmpty(chunk.Choices?.FirstOrDefault()?.Delta?.Content) ? ChatRole.Tool : ChatRole.Assistant,
                    CreatedAt = DateTimeOffset.FromUnixTimeSeconds(chunk.Created),
                    FinishReason = ToFinishReason(chunk),
                    ModelId = modelId,
                };

                if (chunk.Choices?.FirstOrDefault()?.Delta is { } message)
                {
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

                if (ParseErnieChatResponseUsage(chunk) is { } usage)
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

    private async Task CheckBearerTokenAsync(CancellationToken cancellationToken = default)
    {
        if (_token is not null && _token.ExpireTime > DateTimeOffset.Now)
        {
            return;
        }

        _token = await AuthorizeTool.GenerateBearerTokenAsync(_config.AccessKey, _config.SecretKey, cancellationToken).ConfigureAwait(false);
    }

    private ErnieChatRequest ToErnieChatRequest(IList<ChatMessage> chatMessages, ChatOptions? options, bool stream)
    {
        var request = new ErnieChatRequest
        {
            Messages = [.. chatMessages.Select(ToErnieChatMessage)],
            Model = options?.ModelId ?? Metadata.ModelId ?? string.Empty,
            Stream = stream,
        };

        if (options is not null)
        {
            request.MaxCompletionTokens = options.MaxOutputTokens;
            request.Temperature = options.Temperature;
            request.TopP = options.TopP;
            request.PresencePenalty = options.PresencePenalty;
            request.FrequencyPenalty = options.FrequencyPenalty;
            request.Stop = options.StopSequences;
            request.Seed = options.Seed;
            request.ResponseFormat = options.ResponseFormat is not null
                ? options.ResponseFormat is ChatResponseFormatJson ? ErnieResponseFormat.JsonFormat : ErnieResponseFormat.TextFormat
                : null;
        }

        if (options?.AdditionalProperties is { Count: > 0 } additionalProperties)
        {
            foreach (var prop in additionalProperties)
            {
                if (prop.Key == "parallel_tool_calls" && prop.Value is bool ptc)
                {
                    request.ParallelToolCalls = ptc;
                }
                else if (prop.Key == "web_search" && prop.Value is string webParam)
                {
                    request.WebSearch = JsonSerializer.Deserialize(webParam, JsonGenContext.Default.ErnieWebSearchParameters);
                }
            }
        }

        if (options?.Tools is { Count: > 0 } tools)
        {
            request.Tools = [.. tools.Select(ToErnieTool).Where(p => p != null).Select(p => p!)];
        }

        return request;
    }

    private static ErnieChatMessage ToErnieChatMessage(ChatMessage message)
    {
        if (message.Contents?.Count > 0)
        {
            switch (message.Contents[0])
            {
                case TextContent textContent:
                    return new ErnieChatMessage { Role = message.Role.Value, Content = textContent.Text };
                case FunctionCallContent fcc:
                    var sb = new StringBuilder();
                    DictionaryToolkit.WriteDictionary(sb, fcc.Arguments ?? new Dictionary<string, object?>());
                    return new ErnieChatMessage
                    {
                        Role = "assistant",
                        Content = string.Empty,
                        ToolCalls = [new ErnieToolCall
                        {
                            Id = fcc.CallId,
                            Type = "function",
                            Function = new ErnieFunctionCall{
                                Name = fcc.Name,
                                Arguments = sb.ToString(),
                            }
                        }],
                    };
                case FunctionResultContent frc:
                    return new ErnieChatMessage
                    {
                        ToolCallId = frc.CallId,
                        Content = frc.Result?.ToString() ?? string.Empty,
                        Role = "tool",
                    };
                default:
                    break;
            }

            return new ErnieChatMessage { Role = message.Role.Value, Content = message.Text ?? string.Empty };
        }

        throw new NotSupportedException();
    }

    private static ChatMessage FromErnieChoice(ErnieChatResponseChoice choice)
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

        return new ChatMessage(new(role), contents);
    }

    private static ChatFinishReason? ToFinishReason(ErnieChatResponse response) =>
        response.Choices?.First().FinishReason switch
        {
            null => null,
            _ => new(response.Choices?.First().FinishReason!),
        };

    private static ErnieTool? ToErnieTool(AITool tool)
    {
        if (tool is AIFunction function)
        {
            var resultParameters = OpenAIChatToolJson.ZeroFunctionParametersSchema;
            if (function.UnderlyingMethod != null)
            {
                resultParameters = BinaryData.FromString(function.JsonSchema.ToString());
            }

            return new()
            {
                Type = "function",
                Function = new()
                {
                    Name = function.Name,
                    Description = function.Description,
                    Parameters = function.JsonSchema.Deserialize(JsonGenContext.Default.ErnieFunctionToolParameters),
                },
            };
        }

        return default;
    }

    private static FunctionCallContent? ToFunctionCallContent(ErnieToolCall call)
    {
        var callContent = new FunctionCallContent(call.Id, call.Function.Name ?? string.Empty);
        if (call.Function.Arguments is string json)
        {
            var ele = JsonDocument.Parse(json).RootElement;
            callContent.Arguments = JsonToolkit.JsonElementToDictionary(ele);
        }

        return callContent;
    }

    private static UsageDetails? ParseErnieChatResponseUsage(ErnieChatResponse response)
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
