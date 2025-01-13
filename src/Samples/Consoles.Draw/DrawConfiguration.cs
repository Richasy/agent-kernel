// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Consoles.Draw;

internal sealed class DrawConfiguration
{
    [JsonPropertyName("azure_openai")]
    public AzureOpenAIConfiguration? AzureOpenAI { get; set; }

    [JsonPropertyName("ernie")]
    public SecretConfiguration? Ernie { get; set; }

    [JsonPropertyName("hunyuan")]
    public SecretConfiguration? Hunyuan { get; set; }
}

internal sealed class AzureOpenAIConfiguration : KeyConfiguration
{
    [JsonPropertyName("endpoint")]
    public string? Endpoint { get; set; }
}

internal sealed class SecretConfiguration : KeyConfiguration
{
    [JsonPropertyName("secret")]
    public required string Secret { get; set; }
}

internal class KeyConfiguration
{
    [JsonPropertyName("key")]
    [JsonRequired]
    public required string AccessKey { get; set; }

    [JsonPropertyName("model")]
    public string? Model { get; set; }

    [JsonPropertyName("size")]
    public string? Size { get; set; }
}
