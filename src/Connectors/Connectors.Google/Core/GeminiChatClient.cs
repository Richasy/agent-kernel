// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using Richasy.AgentKernel.Connectors.Google.Models;
using Richasy.AgentKernel.Connectors.Google.Models.Core;
using RichasyKernel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace Richasy.AgentKernel.Connectors.Google;

/// <summary>
/// Gemini chat client.
/// </summary>
public sealed class GeminiChatClient : IChatClient
{
    private readonly Uri _chatGenerationEndpoint;
    private readonly Uri _chatStreamingEndpoint;
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="GeminiChatClient"/> class.
    /// </summary>
    public GeminiChatClient(string accessKey, string? modelId = null, Uri? endpoint = null, GeminiVersion version = GeminiVersion.V1Beta)
    {
        endpoint ??= new Uri("https://generativelanguage.googleapis.com");
        Metadata = new("gemini", endpoint, modelId);
        var versionSubLink = GetApiVersionSubLink(version);
        _httpClient = HttpExtensions.CreateHttpClient();
        _chatGenerationEndpoint = new Uri($"{endpoint.ToString().TrimEnd('/')}/{versionSubLink}/models/{modelId}:generateContent?key={accessKey}");
        _chatStreamingEndpoint = new Uri($"{endpoint.ToString().TrimEnd('/')}/{versionSubLink}/models/{modelId}:streamGenerateContent?key={accessKey}&alt=sse");
    }

    /// <inheritdoc/>
    public ChatClientMetadata Metadata { get; }

    /// <inheritdoc/>
    public async Task<ChatResponse> GetResponseAsync(IList<ChatMessage> chatMessages, ChatOptions? options = null, CancellationToken cancellationToken = default)
    {
        var request = GetGeminiRequest(chatMessages, options);
        var response = await GetGeminiResponseAsync(_chatGenerationEndpoint, request, cancellationToken).ConfigureAwait(false);
        var messageContents = GetChatMessageContentsFromResponse(response);
        var firstContent = messageContents[0];
        return new([firstContent])
        {
            ModelId = options?.ModelId ?? Metadata.ModelId,
            CreatedAt = DateTimeOffset.Now,
            FinishReason = ToFinishReason(firstContent.Metadata?.FinishReason),
            Usage = new UsageDetails
            {
                InputTokenCount = response?.UsageMetadata?.PromptTokenCount ?? 0,
                OutputTokenCount = response?.UsageMetadata?.CandidatesTokenCount ?? 0,
                TotalTokenCount = response?.UsageMetadata?.TotalTokenCount ?? 0,
            },
        };
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IList<ChatMessage> chatMessages, ChatOptions? options = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var request = GetGeminiRequest(chatMessages, options);
        HttpResponseMessage? response = null;
        Stream? responseStream = null;
        try
        {
            using var httpRequestMessage = CreateHttpRequest(request, _chatStreamingEndpoint);
            response = await _httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            responseStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception)
        {
            if (response is { IsSuccessStatusCode: false } res)
            {
                var content = await res.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                throw new KernelException($"Failed to get response from Gemini. Status code: {res.StatusCode}. Response: {content}");
            }

            throw;
        }

        using var reader = new StreamReader(responseStream);
        while ((await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false)) is { } line)
        {
            if (line.StartsWith("data:", StringComparison.InvariantCultureIgnoreCase))
            {
                line = line[5..].Trim();
                if (line == "[Done]" || !line.StartsWith('{'))
                {
                    break;
                }

                var geminiResponse = JsonSerializer.Deserialize(line, JsonGenContext.Default.GeminiResponse);
                if (geminiResponse == null)
                {
                    continue;
                }

                var messageContents = GetChatMessageContentsFromResponse(geminiResponse);
                var firstContent = messageContents[0];
                var update = new ChatResponseUpdate
                {
                    Role = firstContent.Role,
                    CreatedAt = DateTimeOffset.Now,
                    FinishReason = ToFinishReason(firstContent.Metadata?.FinishReason),
                    Contents = firstContent.Contents,
                    ModelId = Metadata.ModelId,
                };

                if (firstContent.Metadata != null)
                {
                    update.Contents.Add(new UsageContent(new UsageDetails
                    {
                        InputTokenCount = geminiResponse.UsageMetadata?.PromptTokenCount ?? 0,
                        OutputTokenCount = geminiResponse.UsageMetadata?.CandidatesTokenCount ?? 0,
                        TotalTokenCount = geminiResponse.UsageMetadata?.TotalTokenCount ?? 0,
                    }));
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

    private static ChatFinishReason? ToFinishReason(GeminiFinishReason? reason)
    {
        if (reason == null)
        {
            return null;
        }
        else if (reason == GeminiFinishReason.MaxTokens)
        {
            return ChatFinishReason.Length;
        }
        else if (reason == GeminiFinishReason.Stop)
        {
            return ChatFinishReason.Stop;
        }
        else if (reason == GeminiFinishReason.Safety)
        {
            return ChatFinishReason.ContentFilter;
        }
        else if (reason.Value.Label != null)
        {
            return new ChatFinishReason(reason.Value.Label);
        }

        return null;
    }

    private static string GetApiVersionSubLink(GeminiVersion version)
        => version switch
        {
            GeminiVersion.V1 => "v1",
            GeminiVersion.V1Beta => "v1beta",
            _ => throw new NotSupportedException(),
        };

    private static HttpRequestMessage CreateHttpRequest(GeminiRequest requestData, Uri endpoint)
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint);
        var json = JsonSerializer.Serialize(requestData, JsonGenContext.Default.GeminiRequest);
        httpRequestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
        return httpRequestMessage;
    }

    private static List<GeminiChatMessageContent> GetChatMessageContentsFromResponse(GeminiResponse geminiResponse)
        => geminiResponse.Candidates == null
            ? [new GeminiChatMessageContent { Role = ChatRole.Assistant, Contents = [new TextContent(string.Empty)] }]
            : [.. geminiResponse.Candidates.Select(candidate => GetChatMessageContentFromCandidate(geminiResponse, candidate))];

    private static GeminiChatMessageContent GetChatMessageContentFromCandidate(GeminiResponse geminiResponse, GeminiResponseCandidate candidate)
    {
        var part = candidate.Content?.Parts?[0];
        var msg = new GeminiChatMessageContent { Role = candidate.Content?.Role ?? ChatRole.Assistant, };
        if (part?.FunctionCall != null)
        {
            var argumentJson = part.FunctionCall.Arguments?.ToString() ?? "{}";
            var functionCallContent = new FunctionCallContent(part.FunctionCall.FunctionName, part.FunctionCall.FunctionName)
            {
                Arguments = JsonToolkit.JsonElementToDictionary(JsonDocument.Parse(argumentJson).RootElement)
            };

            msg.Contents = [functionCallContent];
        }
        else if (part?.FunctionResponse != null)
        {
            var resultText = part.FunctionResponse.Response.Content?.ToString();
            var functionResultContent = new FunctionResultContent(part.FunctionResponse.FunctionName, resultText);
            msg.Contents = [functionResultContent];
        }
        else
        {
            msg.Contents = [new TextContent(part?.Text)];
            msg.Metadata = GetResponseMetadata(geminiResponse, candidate);
        }

        return msg;
    }

    private static GeminiMetadata GetResponseMetadata(
        GeminiResponse geminiResponse,
        GeminiResponseCandidate candidate) => new()
        {
            FinishReason = candidate.FinishReason,
            Index = candidate.Index,
            PromptTokenCount = geminiResponse.UsageMetadata?.PromptTokenCount ?? 0,
            CurrentCandidateTokenCount = candidate.TokenCount,
            CandidatesTokenCount = geminiResponse.UsageMetadata?.CandidatesTokenCount ?? 0,
            TotalTokenCount = geminiResponse.UsageMetadata?.TotalTokenCount ?? 0,
            PromptFeedbackBlockReason = geminiResponse.PromptFeedback?.BlockReason,
            PromptFeedbackSafetyRatings = geminiResponse.PromptFeedback?.SafetyRatings.ToList(),
            ResponseSafetyRatings = candidate.SafetyRatings?.ToList(),
        };

    private GeminiRequest GetGeminiRequest(IList<ChatMessage> chatMessages, ChatOptions? options)
    {
        ArgumentNullException.ThrowIfNull(chatMessages);
        var modelId = options?.ModelId ?? Metadata.ModelId;
        var contents = chatMessages.Where(p => p.Role != ChatRole.System).Select(GeminiRequest.CreateGeminiContentFromChatMessage).ToList();
        var request = new GeminiRequest { Contents = contents };
        GeminiRequest.AddConfiguration(options, request);
        request.SafetySettings =
        [
            new(GeminiSafetyCategory.DangerousContent, GeminiSafetyThreshold.BlockNone),
            new(GeminiSafetyCategory.SexuallyExplicit, GeminiSafetyThreshold.BlockNone),
            new(GeminiSafetyCategory.Harassment, GeminiSafetyThreshold.BlockNone),
        ];

        var systemPrompt = chatMessages.FirstOrDefault(x => x.Role == ChatRole.System)?.Text;
        if (!string.IsNullOrEmpty(systemPrompt))
        {
            request.SystemInstruction = new GeminiContent
            {
                Parts = [new GeminiPart { Text = systemPrompt }],
                Role = ChatRole.System,
            };
        }

        if (options?.Tools is { Count: > 0 } tools)
        {
            var actualTools = tools.Select(ToGeminiTool).Where(p => p is not null).Select(p => p!).ToList();
            request.Tools = actualTools?.Count > 0 ? actualTools : null;
        }

        return request;
    }

    private async Task<GeminiResponse> GetGeminiResponseAsync(Uri endpoint, GeminiRequest request, CancellationToken cancellationToken)
    {
        using var requestMessage = CreateHttpRequest(request, endpoint);
        var response = await _httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new KernelException($"Failed to get response from Gemini. Status code: {response.StatusCode}. Response: {responseContent}");
        }

        response.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize(responseContent, JsonGenContext.Default.GeminiResponse)
            ?? throw new KernelException("Failed to deserialize response.");
    }

    private static GeminiTool? ToGeminiTool(AITool tool)
    {
        if (tool is AIFunction function)
        {
            return new GeminiTool
            {
                Functions = [new GeminiTool.FunctionDeclaration
                {
                    Name = function.Name,
                    Description = function.Description,
                    Parameters = function.JsonSchema.Deserialize(JsonGenContext.Default.GeminiFunctionToolParameters),
                }],
            };
        }

        return default;
    }
}
