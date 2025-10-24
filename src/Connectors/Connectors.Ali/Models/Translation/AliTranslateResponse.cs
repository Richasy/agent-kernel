// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Ali.Models.Translation;

internal sealed class AliTranslateResponse
{
    [JsonConverter(typeof(StringFromNumberConverter))]
    public required string Code { get; set; }

    public string? Message { get; set; }

    public string? RequestId { get; set; }

    public AliTranslateResult? Data { get; set; }
}

internal sealed class AliTranslateResult
{
    [JsonConverter(typeof(StringFromNumberConverter))]
    public string? Translated { get; set; }

    public string? WordCount { get; set; }

    public string? DetectedLangauge { get; set; }
}

/// <summary>
/// 将数字转换为字符串的转换器。
/// </summary>
public class StringFromNumberConverter : JsonConverter<string>
{
    /// <inheritdoc/>
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // 如果是字符串正常读
        if (reader.TokenType == JsonTokenType.String)
            return reader.GetString();

        // 如果是数字，则转成字符串
        if (reader.TokenType == JsonTokenType.Number)
#pragma warning disable CA1305 // 指定 IFormatProvider
            return reader.GetDouble().ToString();
#pragma warning restore CA1305 // 指定 IFormatProvider

        // 其它类型为 null 或抛异常
        if (reader.TokenType == JsonTokenType.Null)
            return null;

        throw new JsonException($"Unexpected token {reader.TokenType} when parsing string.");
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}
