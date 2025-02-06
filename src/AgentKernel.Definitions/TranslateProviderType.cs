// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;
using System.Text.Json;

namespace Richasy.AgentKernel;

/// <summary>
/// 翻译器类型.
/// </summary>
[JsonConverter(typeof(TranslateProviderTypeConverter))]
public enum TranslateProviderType
{
    /// <summary>
    /// 阿里云翻译.
    /// </summary>
    Ali,

    /// <summary>
    /// 百度翻译.
    /// </summary>
    Baidu,

    /// <summary>
    /// 有道翻译.
    /// </summary>
    Youdao,

    /// <summary>
    /// Azure翻译.
    /// </summary>
    Azure,

    /// <summary>
    /// 火山翻译.
    /// </summary>
    Volcano,

    /// <summary>
    /// 腾讯翻译.
    /// </summary>
    Tencent,

    /// <summary>
    /// 谷歌翻译.
    /// </summary>
    Google,
}

/// <summary>
/// 翻译类型转换器.
/// </summary>
public sealed class TranslateProviderTypeConverter : JsonConverter<TranslateProviderType>
{
    /// <inheritdoc/>
    public override TranslateProviderType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString()!.ToLower(System.Globalization.CultureInfo.CurrentCulture) switch
        {
            "ali" => TranslateProviderType.Ali,
            "baidu" => TranslateProviderType.Baidu,
            "youdao" => TranslateProviderType.Youdao,
            "azure" => TranslateProviderType.Azure,
            "volcano" => TranslateProviderType.Volcano,
            "tencent" => TranslateProviderType.Tencent,
            "google" => TranslateProviderType.Google,
            _ => throw new JsonException(),
        };
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, TranslateProviderType value, JsonSerializerOptions options)
    {
        var text = value switch
        {
            TranslateProviderType.Ali => "ali",
            TranslateProviderType.Baidu => "baidu",
            TranslateProviderType.Youdao => "youdao",
            TranslateProviderType.Azure => "azure",
            TranslateProviderType.Volcano => "volcano",
            TranslateProviderType.Tencent => "tencent",
            TranslateProviderType.Google => "google",
            _ => throw new JsonException(),
        };

        writer.WriteStringValue(text);
    }
}