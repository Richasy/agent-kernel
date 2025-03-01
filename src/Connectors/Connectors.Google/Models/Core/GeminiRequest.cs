// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using System.Text;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Google.Models.Core;

internal sealed class GeminiRequest
{
    [JsonPropertyName("contents")]
    public IList<GeminiContent> Contents { get; set; } = null!;

    [JsonPropertyName("safetySettings")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IList<GeminiSafetySetting>? SafetySettings { get; set; }

    [JsonPropertyName("generationConfig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ConfigurationElement? Configuration { get; set; }

    [JsonPropertyName("tools")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IList<GeminiTool>? Tools { get; set; }

    [JsonPropertyName("systemInstruction")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public GeminiContent? SystemInstruction { get; set; }

    public static GeminiContent CreateGeminiContentFromChatMessage(ChatMessage message)
    {
        return new GeminiContent
        {
            Parts = CreateGeminiParts(message),
            Role = message.Role
        };
    }

    public static void AddConfiguration(ChatOptions? options, GeminiRequest request)
    {
        if (options == null)
        {
            return;
        }

        request.Configuration = new ConfigurationElement
        {
            Temperature = options.Temperature,
            TopP = options.TopP,
            TopK = options.TopK,
            MaxOutputTokens = options.MaxOutputTokens,
            StopSequences = options.StopSequences,
        };
    }

    public static List<GeminiPart> CreateGeminiParts(ChatMessage message)
    {
        List<GeminiPart> parts = [];
        foreach (var content in message.Contents)
        {
            parts.Add(CreateGeminiPart(content));
        }

        return parts!;
    }

    public static GeminiPart CreateGeminiPart(AIContent content) => content switch
    {
        TextContent textContent => new GeminiPart { Text = textContent.Text },
        DataContent imageContent => CreateGeminiPartFromImage(imageContent),
        FunctionCallContent fcc => CreateGeminiPartFromFunctionCall(fcc),
        FunctionResultContent frc => CreateGeminiPartFromFunctionResult(frc),
        _ => throw new NotSupportedException($"Unsupported content type. {content.GetType().Name} is not supported by Gemini.")
    };

    private static GeminiPart CreateGeminiPartFromImage(DataContent imageContent)
    {
        // Binary data takes precedence over URI as per the ImageContent.ToString() implementation.
        return imageContent.Data is { IsEmpty: false }
            ? new GeminiPart
            {
                InlineData = new GeminiPart.InlineDataPart
                {
                    MimeType = GetMimeTypeFromImageContent(imageContent),
                    InlineData = Convert.ToBase64String(imageContent.Data.Value.ToArray())
                }
            }
            : imageContent.Uri is not null
                ? new GeminiPart
                {
                    FileData = new GeminiPart.FileDataPart
                    {
                        MimeType = GetMimeTypeFromImageContent(imageContent),
                        FileUri = string.IsNullOrEmpty(imageContent.Uri)
                            ? throw new InvalidOperationException("Image content URI is empty.")
                            : new Uri(imageContent.Uri)
                    }
                }
                : throw new InvalidOperationException("Image content does not contain any data or uri.");
    }

    private static GeminiPart CreateGeminiPartFromFunctionCall(FunctionCallContent functionCallContent)
    {
        var sb = new StringBuilder();
        DictionaryToolkit.WriteDictionary(sb, functionCallContent.Arguments ?? new Dictionary<string, object?>());
        return new GeminiPart
        {
            FunctionCall = new GeminiPart.FunctionCallPart
            {
                FunctionName = functionCallContent.Name,
                Arguments = BinaryData.FromString(sb.ToString())
            }
        };
    }

    private static GeminiPart CreateGeminiPartFromFunctionResult(FunctionResultContent functionResultContent)
    {
        return new GeminiPart
        {
            FunctionResponse = new GeminiPart.FunctionResponsePart
            {
                FunctionName = functionResultContent.CallId,
                Response = new GeminiPart.FunctionResponsePart.FunctionResponseEntity
                {
                    Name = functionResultContent.CallId,
                    Content = BinaryData.FromString(functionResultContent.Result?.ToString() ?? string.Empty),
                }
            }
        };
    }

    private static string GetMimeTypeFromImageContent(DataContent imageContent)
    {
        return imageContent.MediaType
               ?? throw new InvalidOperationException("Image content MimeType is empty.");
    }

    internal sealed class ConfigurationElement
    {
        [JsonPropertyName("temperature")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? Temperature { get; set; }

        [JsonPropertyName("topP")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? TopP { get; set; }

        [JsonPropertyName("topK")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? TopK { get; set; }

        [JsonPropertyName("maxOutputTokens")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? MaxOutputTokens { get; set; }

        [JsonPropertyName("stopSequences")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string>? StopSequences { get; set; }

        [JsonPropertyName("candidateCount")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? CandidateCount { get; set; }
    }
}
