// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Consoles.Translation;

internal sealed class TranslationConfiguration
{
    [JsonPropertyName("azure")]
    public AzureConfiguration? Azure { get; set; }
}

internal sealed class AzureConfiguration : KeyConfiguration
{
    [JsonPropertyName("region")]
    public string? Region { get; set; }
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