// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Consoles.Chat;

internal sealed class ChatConfiguration
{
    [JsonPropertyName("azure_openai")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AzureOpenAIConfiguration? AzureOpenAI { get; set; }

    [JsonPropertyName("xai")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public XAIConfiguration? XAI { get; set; }

    [JsonPropertyName("openai")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public OpenAIConfiguration? OpenAI { get; set; }
}

internal sealed class AzureOpenAIConfiguration
{
    [JsonPropertyName("endpoint")]
    [JsonRequired]
    public string? Endpoint { get; set; }

    [JsonPropertyName("key")]
    [JsonRequired]
    public string? AccessKey { get; set; }

    [JsonPropertyName("model")]
    [JsonRequired]
    public string? Model { get; set; }
}

internal sealed class XAIConfiguration
{
    [JsonPropertyName("key")]
    [JsonRequired]
    public string? AccessKey { get; set; }

    [JsonPropertyName("model")]
    [JsonRequired]
    public string? Model { get; set; }
}

internal sealed class OpenAIConfiguration
{
    [JsonPropertyName("key")]
    [JsonRequired]
    public string? AccessKey { get; set; }

    [JsonPropertyName("model")]
    [JsonRequired]
    public string? Model { get; set; }

    [JsonPropertyName("endpoint")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Endpoint { get; set; }

    [JsonPropertyName("organization")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Organization { get; set; }
}