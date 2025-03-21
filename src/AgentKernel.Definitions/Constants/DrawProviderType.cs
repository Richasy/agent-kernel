// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;
using System.Text.Json;

namespace Richasy.AgentKernel;

/// <summary>
/// 绘画提供程序类型.
/// </summary>
[JsonConverter(typeof(DrawProviderTypeConverter))]
public enum DrawProviderType
{
    /// <summary>
    /// OpenAI.
    /// </summary>
    OpenAI,

    /// <summary>
    /// Azure OpenAI.
    /// </summary>
    AzureOpenAI,

    /// <summary>
    /// 文心一言.
    /// </summary>
    Ernie,

    /// <summary>
    /// 腾讯混元.
    /// </summary>
    Hunyuan,

    /// <summary>
    /// 讯飞星火.
    /// </summary>
    Spark,

    /// <summary>
    /// x.AI.
    /// </summary>
    XAI,

    /// <summary>
    /// 智谱.
    /// </summary>
    ZhiPu,
}

/// <summary>
/// 服务类型转换器.
/// </summary>
public sealed class DrawProviderTypeConverter : JsonConverter<DrawProviderType>
{
    /// <inheritdoc/>
    public override DrawProviderType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString()!.ToLower(System.Globalization.CultureInfo.CurrentCulture) switch
        {
            "openai" => DrawProviderType.OpenAI,
            "azure_openai" => DrawProviderType.AzureOpenAI,
            "ernie" => DrawProviderType.Ernie,
            "hunyuan" => DrawProviderType.Hunyuan,
            "spark" => DrawProviderType.Spark,
            "xai" => DrawProviderType.XAI,
            "zhipu" => DrawProviderType.ZhiPu,
            _ => throw new JsonException(),
        };
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, DrawProviderType value, JsonSerializerOptions options)
    {
        var text = value switch
        {
            DrawProviderType.OpenAI => "openai",
            DrawProviderType.AzureOpenAI => "azure_openai",
            DrawProviderType.Ernie => "ernie",
            DrawProviderType.Hunyuan => "hunyuan",
            DrawProviderType.Spark => "spark",
            DrawProviderType.XAI => "xai",
            DrawProviderType.ZhiPu => "zhipu",
            _ => throw new JsonException(),
        };

        writer.WriteStringValue(text);
    }
}