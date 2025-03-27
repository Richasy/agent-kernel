// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;
using System.Text.Json;

namespace Richasy.AgentKernel;

/// <summary>
/// 讲述人类型.
/// </summary>
[JsonConverter(typeof(AudioProviderTypeConverter))]
public enum AudioProviderType
{
    /// <summary>
    /// Open AI.
    /// </summary>
    OpenAI,

    /// <summary>
    /// Azure Open AI.
    /// </summary>
    AzureOpenAI,

    /// <summary>
    /// Azure 语音服务.
    /// </summary>
    Azure,

    /// <summary>
    /// Edge 语音服务.
    /// </summary>
    Edge,

    /// <summary>
    /// Windows 语音服务.
    /// </summary>
    Windows,

    /// <summary>
    /// 火山语音服务.
    /// </summary>
    Volcano,

    /// <summary>
    /// 腾讯语音服务.
    /// </summary>
    Tencent,
}

/// <summary>
/// 服务类型转换器.
/// </summary>
public sealed class AudioProviderTypeConverter : JsonConverter<AudioProviderType>
{
    /// <inheritdoc/>
    public override AudioProviderType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString()!.ToLower(System.Globalization.CultureInfo.CurrentCulture) switch
        {
            "openai" => AudioProviderType.OpenAI,
            "azure_openai" or "azureopenai" => AudioProviderType.AzureOpenAI,
            "azure_speech" or "azurespeech" or "azure" => AudioProviderType.Azure,
            "edge_speech" or "edgespeech" or "edge" => AudioProviderType.Edge,
            "windows_speech" or "windowsspeech" or "windows" => AudioProviderType.Windows,
            "volcano" => AudioProviderType.Volcano,
            "tencent" => AudioProviderType.Tencent,
            _ => throw new JsonException(),
        };
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, AudioProviderType value, JsonSerializerOptions options)
    {
        var text = value switch
        {
            AudioProviderType.OpenAI => "openai",
            AudioProviderType.AzureOpenAI => "azure_openai",
            AudioProviderType.Azure => "azure",
            AudioProviderType.Edge => "edge",
            AudioProviderType.Windows => "windows",
            AudioProviderType.Volcano => "volcano",
            AudioProviderType.Tencent => "tencent",
            _ => throw new JsonException(),
        };

        writer.WriteStringValue(text);
    }
}
