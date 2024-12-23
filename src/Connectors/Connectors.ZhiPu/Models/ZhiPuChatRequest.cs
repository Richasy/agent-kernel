// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.ZhiPu.Models;

internal abstract class ZhiPuChatRequest
{
    public required string Model { get; set; }

    public string? RequestId { get; set; }

    public bool? DoSample { get; set; }

    public bool? Stream { get; set; }

    public double? Temperature { get; set; }

    public double? TopP { get; set; }

    public int? MaxTokens { get; set; }

    public string? UserId { get; set; }
}

internal sealed class ZhiPuBasicChatRequest : ZhiPuChatRequest
{
    [JsonConverter(typeof(ZhiPuChatListConverter))]
    public required IList<object> Messages { get; set; }

    public IList<ZhiPuTool>? Tools { get; set; }

    public ZhiPuResponseFormat? ResponseFormat { get; set; }

    public string[]? Stop { get; set; }
}

internal sealed class ZhiPuContentChatRequest : ZhiPuChatRequest
{
    public required IList<ZhiPuChatRequestContentMessage> Messages { get; set; }
}

internal sealed class ZhiPuChatListConverter : JsonConverter<IList<object>>
{
    public override IList<object>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var list = new List<object>();
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException();
        }

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                return list;
            }

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            using var doc = JsonDocument.ParseValue(ref reader);
            var jobj = doc.RootElement;
            if (jobj.TryGetProperty("tool_call_id", out _))
            {
                list.Add(JsonSerializer.Deserialize(jobj.GetRawText(), JsonGenerationContext.Default.ZhiPuChatRequestToolMessage)!);
            }
            else if (jobj.TryGetProperty("content", out var contentElement))
            {
                if (contentElement.ValueKind == JsonValueKind.String)
                {
                    list.Add(JsonSerializer.Deserialize(jobj.GetRawText(), JsonGenerationContext.Default.ZhiPuChatRequestBasicMessage)!);
                }
                else if (contentElement.ValueKind == JsonValueKind.Array)
                {
                    list.Add(JsonSerializer.Deserialize(jobj.GetRawText(), JsonGenerationContext.Default.ZhiPuChatRequestContentMessage)!);
                }
                else
                {
                    throw new JsonException();
                }
            }
            else
            {
                throw new JsonException();
            }
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, IList<object> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var item in value)
        {
            if (item is ZhiPuChatRequestBasicMessage basicMessage)
            {
                JsonSerializer.Serialize(writer, basicMessage, JsonGenerationContext.Default.ZhiPuChatRequestBasicMessage);
            }
            else if (item is ZhiPuChatRequestContentMessage contentMessage)
            {
                JsonSerializer.Serialize(writer, contentMessage, JsonGenerationContext.Default.ZhiPuChatRequestContentMessage);
            }
            else if (item is ZhiPuChatRequestToolMessage toolMessage)
            {
                JsonSerializer.Serialize(writer, toolMessage, JsonGenerationContext.Default.ZhiPuChatRequestToolMessage);
            }
            else if (item is ZhiPuChatRequestAssistantMessage assistantMessage)
            {
                JsonSerializer.Serialize(writer, assistantMessage, JsonGenerationContext.Default.ZhiPuChatRequestAssistantMessage);
            }
            else
            {
                throw new JsonException($"Unsupported type: {item.GetType()}");
            }
        }

        writer.WriteEndArray();
    }
}