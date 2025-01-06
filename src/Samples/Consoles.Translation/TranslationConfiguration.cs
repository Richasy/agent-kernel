// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Consoles.Translation;

internal sealed class TranslationConfiguration
{
    [JsonPropertyName("azure")]
    public AzureConfiguration? Azure { get; set; }

    [JsonPropertyName("ali")]
    public AliConfiguration? Ali { get; set; }
}

internal sealed class AzureConfiguration : KeyConfiguration
{
    [JsonPropertyName("region")]
    public string? Region { get; set; }
}

internal sealed class AliConfiguration : KeyConfiguration
{
    [JsonPropertyName("secret")]
    public string? Secret { get; set; }
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