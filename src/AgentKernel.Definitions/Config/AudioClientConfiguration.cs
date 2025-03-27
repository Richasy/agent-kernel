// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel;

/// <summary>
/// 音频客户端配置.
/// </summary>
public sealed class AudioClientConfiguration
{
    /// <summary>
    /// Open AI 客户端配置.
    /// </summary>
    [JsonPropertyName("openai")]
    public OpenAIAudioConfig? OpenAI { get; set; }

    /// <summary>
    /// Azure Open AI 客户端配置.
    /// </summary>
    [JsonPropertyName("azure_openai")]
    public AzureOpenAIAudioConfig? AzureOpenAI { get; set; }

    /// <summary>
    /// Azure 语音客户端配置.
    /// </summary>
    [JsonPropertyName("azure")]
    public AzureAudioConfig? AzureSpeech { get; set; }

    /// <summary>
    /// 火山语音客户端配置.
    /// </summary>
    [JsonPropertyName("volcano")]
    public VolcanoAudioConfig? Volcano { get; set; }
}

/// <summary>
/// Open AI 客户端配置.
/// </summary>
public class OpenAIAudioConfig : AudioClientEndpointConfigBase
{
    /// <summary>
    /// 组织 ID.
    /// </summary>
    [JsonPropertyName("organization")]
    public string? OrganizationId { get; set; }
}

/// <summary>
/// Azure Open AI 客户端配置.
/// </summary>
public class AzureOpenAIAudioConfig : AudioClientEndpointConfigBase
{
    /// <inheritdoc/>
    public override bool IsValid()
    {
        return base.IsValid()
            && !string.IsNullOrEmpty(Endpoint);
    }
}

/// <summary>
/// Azure 语音客户端配置.
/// </summary>
public class AzureAudioConfig : AudioClientConfigBase
{
    /// <summary>
    /// 地区.
    /// </summary>
    [JsonPropertyName("region")]
    public string? Region { get; set; }

    /// <inheritdoc/>
    public override bool IsValid()
        => base.IsValid() && !string.IsNullOrEmpty(Region);
}

/// <summary>
/// 火山语音客户端配置.
/// </summary>
public class VolcanoAudioConfig : AudioClientConfigBase
{
    /// <summary>
    /// 应用 ID.
    /// </summary>
    [JsonPropertyName("appId")]
    public string? AppId { get; set; }

    /// <inheritdoc/>
    public override bool IsValid()
        => base.IsValid() && !string.IsNullOrEmpty(AppId);
}

/// <summary>
/// 客户端配置基类.
/// </summary>
public abstract class AudioClientConfigBase : ConfigBase
{
    /// <summary>
    /// 访问密钥.
    /// </summary>
    [JsonPropertyName("key")]
    public string? Key { get; set; }

    /// <summary>
    /// 自定义模型列表.
    /// </summary>
    [JsonPropertyName("models")]
    public IList<AudioModel>? CustomModels { get; set; }

    /// <summary>
    /// 是否有效.
    /// </summary>
    /// <returns>配置是否有效.</returns>
    public virtual bool IsValid()
        => !string.IsNullOrEmpty(Key);

    /// <summary>
    /// 自定义模型是否不为空.
    /// </summary>
    /// <returns>是否不为空.</returns>
    public bool IsCustomModelNotEmpty()
        => CustomModels?.Count > 0;
}

/// <summary>
/// 客户端终结点配置基类.
/// </summary>
public abstract class AudioClientEndpointConfigBase : AudioClientConfigBase
{
    /// <summary>
    /// 终结点.
    /// </summary>
    [JsonPropertyName("endpoint")]
    public string? Endpoint { get; set; }
}
