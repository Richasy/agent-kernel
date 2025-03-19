using System.Text.Json;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Core.Mcp.Utils.Json;

internal sealed class BinaryJsonSchemaConverter : JsonConverter<BinaryData?>
{
    /// <inheritdoc/>
    public override BinaryData? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var jsonString = reader.GetString();
            if (string.IsNullOrEmpty(jsonString))
            {
                return default;
            }
        }

        using var jsonDoc = JsonDocument.ParseValue(ref reader);
        return BinaryData.FromString(jsonDoc.RootElement.GetRawText());
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, BinaryData? value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }

        var content = value.ToString();
        if (content.StartsWith('{') || content.StartsWith('['))
        {
            var jsonEle = JsonDocument.Parse(content).RootElement;
            jsonEle.WriteTo(writer);
        }
        else if (content == "null")
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStringValue(content);
        }
    }
}
