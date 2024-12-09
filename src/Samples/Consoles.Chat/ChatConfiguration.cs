// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Consoles.Chat;

internal sealed class ChatConfiguration
{
    [JsonPropertyName("azure_openai")]
    public AzureOpenAIConfiguration? AzureOpenAI { get; set; }
}

internal sealed class AzureOpenAIConfiguration
{
    [JsonPropertyName("endpoint")]
    public string? Endpoint { get; set; }

    [JsonPropertyName("key")]
    public string? AccessKey { get; set; }

    [JsonPropertyName("model")]
    public string? Model { get; set; }
}