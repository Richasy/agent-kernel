// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel;

/// <summary>
/// JsonSchema 和 BinaryData 转换器.
/// </summary>
public class BinaryJsonSchemaConverter : JsonConverter<BinaryData?>
{
    /// <inheritdoc/>
    public override BinaryData? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // 读取 JSON 并转换为 BinaryData
        var jsonString = reader.GetString();
        if (string.IsNullOrEmpty(jsonString))
        {
            return default;
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

        var jsonEle = JsonDocument.Parse(value.ToString()).RootElement;
        jsonEle.WriteTo(writer);
    }
}
