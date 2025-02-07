// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Richasy.AgentKernel;

/// <summary>
/// 翻译客户端的配置.
/// </summary>
public sealed class TranslateClientConfiguration
{
    /// <summary>
    /// Azure 翻译配置.
    /// </summary>
    [JsonPropertyName("azure")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AzureTranslateConfig? Azure { get; set; }

    /// <summary>
    /// 百度翻译配置.
    /// </summary>
    [JsonPropertyName("baidu")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BaiduTranslateConfig? Baidu { get; set; }

    /// <summary>
    /// 有道翻译配置.
    /// </summary>
    [JsonPropertyName("youdao")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public YoudaoTranslateConfig? Youdao { get; set; }

    /// <summary>
    /// 腾讯翻译配置.
    /// </summary>
    [JsonPropertyName("tencent")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TencentTranslateConfig? Tencent { get; set; }

    /// <summary>
    /// 阿里翻译配置.
    /// </summary>
    [JsonPropertyName("ali")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AliTranslateConfig? Ali { get; set; }

    /// <summary>
    /// 火山翻译配置.
    /// </summary>
    [JsonPropertyName("volcano")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public VolcanoTranslateConfig? Volcano { get; set; }
}

/// <summary>
/// 阿里翻译客户端配置.
/// </summary>
public class AliTranslateConfig : TranslateClientConfigBase
{
    /// <summary>
    /// 密钥.
    /// </summary>
    [JsonPropertyName("secret")]
    public string? Secret { get; set; }

    /// <inheritdoc/>
    public override bool IsValid()
        => base.IsValid() && !string.IsNullOrEmpty(Secret);
}

/// <summary>
/// Azure 翻译客户端配置.
/// </summary>
public class AzureTranslateConfig : TranslateClientConfigBase
{
    /// <summary>
    /// 区域.
    /// </summary>
    [JsonPropertyName("region")]
    public string? Region { get; set; }

    /// <inheritdoc/>
    public override bool IsValid()
        => base.IsValid() && !string.IsNullOrEmpty(Region);
}

/// <summary>
/// 百度翻译客户端配置.
/// </summary>
public class BaiduTranslateConfig : AppTranslateClientConfigBase
{
}

/// <summary>
/// 有道翻译客户端配置.
/// </summary>
public class YoudaoTranslateConfig : AppTranslateClientConfigBase
{
}

/// <summary>
/// 腾讯翻译客户端配置.
/// </summary>
public class TencentTranslateConfig : TranslateClientConfigBase
{
    /// <summary>
    /// 密钥标识.
    /// </summary>
    [JsonPropertyName("secret_id")]
    public string? SecretId { get; set; }

    /// <inheritdoc/>
    public override bool IsValid()
        => base.IsValid() && !string.IsNullOrEmpty(SecretId);
}

/// <summary>
/// 火山翻译客户端配置.
/// </summary>
public class VolcanoTranslateConfig : TranslateClientConfigBase
{
    /// <summary>
    /// 密钥标识.
    /// </summary>
    [JsonPropertyName("key_id")]
    public string? KeyId { get; set; }

    /// <inheritdoc/>
    public override bool IsValid()
        => base.IsValid() && !string.IsNullOrEmpty(KeyId);
}

/// <summary>
/// 应用客户端配置基类.
/// </summary>
public abstract class AppTranslateClientConfigBase : TranslateClientConfigBase
{
    /// <summary>
    /// 应用标识.
    /// </summary>
    [JsonPropertyName("app_id")]
    public string? AppId { get; set; }

    /// <inheritdoc/>
    public override bool IsValid()
        => base.IsValid() && !string.IsNullOrEmpty(AppId);
}

/// <summary>
/// 客户端配置基类.
/// </summary>
public abstract class TranslateClientConfigBase : ConfigBase
{
    /// <summary>
    /// 访问密钥.
    /// </summary>
    [JsonPropertyName("key")]
    public string? Key { get; set; }

    /// <summary>
    /// 是否有效.
    /// </summary>
    /// <returns>配置是否有效.</returns>
    public virtual bool IsValid()
        => !string.IsNullOrEmpty(Key);
}
