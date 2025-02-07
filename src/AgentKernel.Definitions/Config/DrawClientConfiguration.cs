using Richasy.AgentKernel.Models;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel;

/// <summary>
/// 绘画客户端配置.
/// </summary>
public sealed class DrawClientConfiguration
{
    /// <summary>
    /// Open AI 客户端配置.
    /// </summary>
    [JsonPropertyName("openai")]
    public OpenAIDrawConfig? OpenAI { get; set; }

    /// <summary>
    /// Azure OpenAI 客户端配置.
    /// </summary>
    [JsonPropertyName("azure_openai")]
    public AzureOpenAIDrawConfig? AzureOpenAI { get; set; }

    /// <summary>
    /// 文心一言客户端配置.
    /// </summary>
    [JsonPropertyName("ernie")]
    public ErnieDrawConfig? Ernie { get; set; }

    /// <summary>
    /// 混元客户端配置.
    /// </summary>
    [JsonPropertyName("hunyuan")]
    public HunyuanDrawConfig? Hunyuan { get; set; }

    /// <summary>
    /// 星火客户端配置.
    /// </summary>
    [JsonPropertyName("spark")]
    public SparkDrawConfig? Spark { get; set; }
}

/// <summary>
/// Open AI 客户端配置.
/// </summary>
public sealed class OpenAIDrawConfig : DrawClientEndpointConfigBase
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
public sealed class AzureOpenAIDrawConfig : DrawClientEndpointConfigBase
{
    /// <inheritdoc/>
    public override bool IsValid()
    {
        return base.IsValid()
            && !string.IsNullOrEmpty(Endpoint);
    }
}

/// <summary>
/// 文心一言客户端配置.
/// </summary>
public sealed class ErnieDrawConfig : DrawClientConfigBase
{
    /// <summary>
    /// 密钥.
    /// </summary>
    [JsonPropertyName("secret")]
    public required string Secret { get; set; }

    /// <inheritdoc/>
    public override bool IsValid()
        => base.IsValid() && !string.IsNullOrEmpty(Secret);
}

/// <summary>
/// 混元客户端配置.
/// </summary>
public sealed class HunyuanDrawConfig : DrawClientConfigBase
{
    /// <summary>
    /// 密钥.
    /// </summary>
    [JsonPropertyName("secret")]
    public required string Secret { get; set; }

    /// <inheritdoc/>
    public override bool IsValid()
        => base.IsValid() && !string.IsNullOrEmpty(Secret);
}

/// <summary>
/// 星火客户端配置.
/// </summary>
public sealed class SparkDrawConfig : DrawClientConfigBase
{
    /// <summary>
    /// 密钥.
    /// </summary>
    [JsonPropertyName("secret")]
    public required string Secret { get; set; }

    /// <summary>
    /// 应用标识.
    /// </summary>
    [JsonPropertyName("app_id")]
    public required string AppId { get; set; }

    /// <inheritdoc/>
    public override bool IsValid()
        => base.IsValid() && !string.IsNullOrEmpty(Secret) && !string.IsNullOrEmpty(AppId);
}

/// <summary>
/// 客户端配置基类.
/// </summary>
public abstract class DrawClientConfigBase : ConfigBase
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
public abstract class DrawClientEndpointConfigBase : DrawClientConfigBase
{
    /// <summary>
    /// 终结点.
    /// </summary>
    [JsonPropertyName("endpoint")]
    public string? Endpoint { get; set; }
}