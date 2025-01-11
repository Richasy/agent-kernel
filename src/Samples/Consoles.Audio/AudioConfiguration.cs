// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Consoles.Audio;

internal sealed class AudioConfiguration
{
    [JsonPropertyName("azure")]
    public AzureConfiguration? Azure { get; set; }

    [JsonPropertyName("edge")]
    public KeyConfiguration? Edge { get; set; }

    [JsonPropertyName("azure_openai")]
    public AzureOpenAIConfiguration? AzureOpenAI { get; set; }
}

internal sealed class AzureConfiguration : KeyConfiguration
{
    [JsonPropertyName("region")]
    public string? Region { get; set; }
}

internal sealed class AzureOpenAIConfiguration : KeyConfiguration
{
    [JsonPropertyName("endpoint")]
    public string? Endpoint { get; set; }

    [JsonPropertyName("model")]
    public string? Model { get; set; }
}

internal class KeyConfiguration
{
    [JsonPropertyName("key")]
    [JsonRequired]
    public string? AccessKey { get; set; }

    [JsonPropertyName("language")]
    public string? Language { get; set; }

    [JsonPropertyName("voice")]
    public string? Voice { get; set; }
}