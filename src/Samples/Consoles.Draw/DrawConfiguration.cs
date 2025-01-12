// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Consoles.Draw;

internal sealed class DrawConfiguration
{
    [JsonPropertyName("azure_openai")]
    public AzureOpenAIConfiguration? AzureOpenAI { get; set; }
}

internal sealed class AzureOpenAIConfiguration : KeyConfiguration
{
    [JsonPropertyName("endpoint")]
    public string? Endpoint { get; set; }
}

internal class KeyConfiguration
{
    [JsonPropertyName("key")]
    [JsonRequired]
    public string? AccessKey { get; set; }

    [JsonPropertyName("model")]
    public string? Model { get; set; }

    [JsonPropertyName("size")]
    public string? Size { get; set; }
}
