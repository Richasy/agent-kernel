// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Core.Mcp;

/// <summary>
/// JsonSchema 和 BinaryData 转换器.
/// </summary>
internal sealed class BinaryJsonSchemaConverter : JsonConverter<BinaryData?>
{
    /// <inheritdoc/>
    public override BinaryData? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // 读取 JSON 并转换为 BinaryData
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
        // 将 BinaryData 转换为字符串并写入 JSON
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }

        var content = value.ToString();
        // 替换属性值中的单引号为双引号
        content = content.Replace("'", "\"", StringComparison.InvariantCultureIgnoreCase);

        if (content.StartsWith('{') || content.StartsWith('['))
        {
            var jsonEle = JsonDocument.Parse(content).RootElement;
            jsonEle.WriteTo(writer);
        }
        else
        {
            writer.WriteStringValue(content);
        }
    }
}
