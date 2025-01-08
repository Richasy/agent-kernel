// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Consoles.Translation;

internal sealed class TranslationConfiguration
{
    [JsonPropertyName("azure")]
    public AzureConfiguration? Azure { get; set; }

    [JsonPropertyName("ali")]
    public SecretConfiguration? Ali { get; set; }

    [JsonPropertyName("baidu")]
    public SecretConfiguration? Baidu { get; set; }

    [JsonPropertyName("tencent")]
    public IdConfiguration? Tencent { get; set; }

    [JsonPropertyName("volcano")]
    public IdConfiguration? Volcano { get; set; }

    [JsonPropertyName("youdao")]
    public IdConfiguration? Youdao { get; set; }

    [JsonPropertyName("google")]
    public KeyConfiguration? Google { get; set; }
}

internal sealed class AzureConfiguration : KeyConfiguration
{
    [JsonPropertyName("region")]
    public string? Region { get; set; }
}

internal sealed class SecretConfiguration : KeyConfiguration
{
    [JsonPropertyName("secret")]
    public string? Secret { get; set; }
}

internal sealed class IdConfiguration : KeyConfiguration
{
    [JsonPropertyName("id")]
    public string? SecretId { get; set; }
}

internal class KeyConfiguration
{
    [JsonPropertyName("key")]
    [JsonRequired]
    public string? AccessKey { get; set; }

    [JsonPropertyName("target")]
    public string? TargetLanguage { get; set; }

    [JsonPropertyName("source")]
    public string? SourceLanguage { get; set; }
}