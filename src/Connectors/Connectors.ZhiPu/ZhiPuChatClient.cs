// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using Richasy.AgentKernel.Connectors.ZhiPu.Models;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace Richasy.AgentKernel.Connectors.ZhiPu;

/// <summary>
/// 智谱聊天客户端.
/// </summary>
public sealed class ZhiPuChatClient : IChatClient
{
    // private static readonly JsonElement _defaultParameterSchema = JsonDocument.Parse("{}").RootElement;

    private const string _apiEndpoint = "https://open.bigmodel.cn/api/paas/v4/chat/completions";
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _toolCallJsonSerializerOptions = AIJsonUtilities.DefaultOptions;

    /// <summary>
    /// 初始化 <see cref="ZhiPuChatClient"/> 类的新实例.
    /// </summary>
    public ZhiPuChatClient(string accessKey, string? modelId = null)
    {
        if (modelId != null && string.IsNullOrWhiteSpace(modelId))
        {
            throw new ArgumentNullException(nameof(modelId));
        }

        _httpClient = HttpExtensions.CreateHttpClient();
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessKey);
        Metadata = new("zhipu", new Uri(_apiEndpoint), modelId);
    }

    /// <inheritdoc/>
    public ChatClientMetadata Metadata { get; }

    /// <inheritdoc/>
    public async Task<Microsoft.Extensions.AI.ChatCompletion> CompleteAsync(IList<ChatMessage> chatMessages, ChatOptions? options = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(chatMessages);

        var chatRequest = ToZhiPuChatRequest(chatMessages, options, stream: false);
        var json = chatRequest is ZhiPuBasicChatRequest basicRequest
            ? JsonSerializer.Serialize(basicRequest, JsonGenerationContext.Default.ZhiPuBasicChatRequest)
            : JsonSerializer.Serialize((ZhiPuContentChatRequest)chatRequest, JsonGenerationContext.Default.ZhiPuContentChatRequest);
        using var httpResponse = await _httpClient.PostAsync(
            new Uri(_apiEndpoint),
            new StringContent(json, Encoding.UTF8, "application/json"),
            cancellationToken).ConfigureAwait(false);

        var responseText = await httpResponse.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (responseText.StartsWith("{\"error\"", StringComparison.InvariantCultureIgnoreCase))
        {
            var errorResponse = JsonSerializer.Deserialize(responseText, JsonGenerationContext.Default.ZhiPuErrorResponse);
            throw new KernelException(errorResponse?.Error?.Message ?? "Unknown error");
        }

        var response = JsonSerializer.Deserialize(responseText, JsonGenerationContext.Default.ZhiPuChatResponse)!;
        return new([FromZhiPuMessage(response.Choices?.First() ?? throw new KernelException("Empty response"))])
        {
            CompletionId = response.Id,
            ModelId = response.Model ?? options?.ModelId ?? Metadata.ModelId,
            CreatedAt = DateTimeOffset.FromUnixTimeSeconds(response.Created),
            FinishReason = ToFinishReason(response),
            Usage = ParseZhiPuChatResponseUsage(response),
        };
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<StreamingChatCompletionUpdate> CompleteStreamingAsync(IList<ChatMessage> chatMessages, ChatOptions? options = null, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    /// <inheritdoc/>
    public void Dispose()
        => _httpClient?.Dispose();

    /// <inheritdoc/>
    public object? GetService(Type serviceType, object? serviceKey = null)
    {
        ArgumentNullException.ThrowIfNull(serviceType);
        return serviceKey is null && serviceType.IsInstanceOfType(this) ? this : null;
    }

    private ZhiPuChatRequest ToZhiPuChatRequest(IList<ChatMessage> chatMessages, ChatOptions? options, bool stream)
    {
        var isVisualModel = options?.AdditionalProperties?.TryGetValue("visual", out var isVisualModelValue) is true && Convert.ToBoolean(isVisualModelValue, CultureInfo.InvariantCulture);
        ZhiPuChatRequest request = isVisualModel
            ? new ZhiPuContentChatRequest()
            {
                Messages = chatMessages.Select(x => ToZhiPuChatRequestMessages(x, useContentMessage: true)).OfType<ZhiPuChatRequestContentMessage>().ToList() ?? [],
                Model = options?.ModelId ?? Metadata.ModelId ?? string.Empty,
                Stream = stream,
            }
            : new ZhiPuBasicChatRequest()
            {
                ResponseFormat = options?.ResponseFormat is ChatResponseFormatJson ? ZhiPuResponseFormat.JsonFormat : default,
                Messages = chatMessages.Select(x => ToZhiPuChatRequestMessages(x, useContentMessage: false)).ToList() ?? [],
                Model = options?.ModelId ?? Metadata.ModelId ?? string.Empty,
                Stream = stream,
            };

        if (options is not null)
        {
            request.MaxTokens = options.MaxOutputTokens;
            request.Temperature = options.Temperature;
            request.TopP = options.TopP;
            if (options.AdditionalProperties?.TryGetValue("do_sample", out var doSample) is true)
            {
                request.DoSample = Convert.ToBoolean(doSample, CultureInfo.InvariantCulture);
            }
        }

        return request;
    }

    private object ToZhiPuChatRequestMessages(ChatMessage content, bool useContentMessage)
    {
        if (useContentMessage)
        {
            var contentMsg = new ZhiPuChatRequestContentMessage { Role = content.Role.Value, Content = [] };
            foreach (var item in content.Contents)
            {
                switch (item)
                {
                    case TextContent textContent:
                        contentMsg.Content.Add(ZhiPuChatContent.CreateTextMessage(textContent.Text));
                        break;
                    case ImageContent imgContent when imgContent.Uri != null:
                        contentMsg.Content.Add(ZhiPuChatContent.CreateImageMessage(imgContent.Uri));
                        break;
                    default:
                        break;
                }
            }

            return contentMsg;
        }
        else if (content.Contents.Count > 0)
        {
            switch (content.Contents[0])
            {
                case TextContent textContent:
                    return new ZhiPuChatRequestBasicMessage { Role = content.Role.Value, Content = textContent.Text };
                case FunctionCallContent fcc:
                    return new ZhiPuChatResponseMessage
                    {
                        Role = "assistant",
                        Content = string.Empty,
                        ToolCalls = [new ZhiPuToolCall {
                            Id = fcc.CallId,
                            Type = "function",
                            Function = new ZhiPuFunctionToolCall {
                                Name = fcc.Name,
                                Arguments = JsonSerializer.SerializeToElement(fcc.Arguments, _toolCallJsonSerializerOptions.GetTypeInfo(typeof(IDictionary<string, object?>))),
                            }}],
                    };
                case FunctionResultContent frc:
                    return new ZhiPuChatRequestToolMessage { ToolCallId = frc.CallId, Content = frc.Result?.ToString() ?? string.Empty, Role = "tool" };
                default:
                    break;
            }

            var text = content.Text;
            return new ZhiPuChatRequestBasicMessage { Role = content.Role.Value, Content = text };
        }

        throw new NotSupportedException();
    }

    private static ChatMessage FromZhiPuMessage(ZhiPuChatResponseChoice choice)
    {
        var message = choice.Message ?? choice.Delta ?? throw new KernelException("Empty response");
        List<AIContent> contents = [];

        // TODO: 处理工具调用.

        if (message.Content?.Length > 0 || contents.Count == 0)
        {
            contents.Insert(0, new TextContent(message.Content));
        }

        return new ChatMessage(new(message.Role), contents);
    }

    private static ChatFinishReason? ToFinishReason(ZhiPuChatResponse response) =>
        response.Choices?.First().FinishReason switch
        {
            null => null,
            "stop" => ChatFinishReason.Stop,
            "length" => ChatFinishReason.Length,
            _ => new(response.Choices?.First().FinishReason ?? "empty")
        };

    private static UsageDetails? ParseZhiPuChatResponseUsage(ZhiPuChatResponse response)
    {
        return response.Usage == null
            ? null
            : new UsageDetails
            {
                InputTokenCount = response.Usage.PromptTokens,
                OutputTokenCount = response.Usage.CompletionTokens,
                TotalTokenCount = response.Usage.TotalTokens,
            };
    }
}
