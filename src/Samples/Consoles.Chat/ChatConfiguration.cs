// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Consoles.Chat;

internal sealed class ChatConfiguration
{
    [JsonPropertyName("azure_openai")]
    public EndpointConfiguration? AzureOpenAI { get; set; }

    [JsonPropertyName("xai")]
    public KeyConfiguration? XAI { get; set; }

    [JsonPropertyName("openai")]
    public OpenAIConfiguration? OpenAI { get; set; }

    [JsonPropertyName("zhipu")]
    public KeyConfiguration? ZhiPu { get; set; }

    [JsonPropertyName("lingyi")]
    public KeyConfiguration? LingYi { get; set; }

    [JsonPropertyName("anthropic")]
    public EndpointConfiguration? Anthropic { get; set; }

    [JsonPropertyName("moonshot")]
    public KeyConfiguration? Moonshot { get; set; }

    [JsonPropertyName("gemini")]
    public EndpointConfiguration? Gemini { get; set; }

    [JsonPropertyName("deepseek")]
    public KeyConfiguration? DeepSeek { get; set; }

    [JsonPropertyName("qwen")]
    public KeyConfiguration? Qwen { get; set; }

    [JsonPropertyName("ernie")]
    public KeyConfiguration? Ernie { get; set; }

    [JsonPropertyName("hunyuan")]
    public KeyConfiguration? Hunyuan { get; set; }

    [JsonPropertyName("spark")]
    public KeyConfiguration? Spark { get; set; }

    [JsonPropertyName("doubao")]
    public KeyConfiguration? Doubao { get; set; }

    [JsonPropertyName("siliconflow")]
    public KeyConfiguration? SiliconFlow { get; set; }
}

internal sealed class OpenAIConfiguration : EndpointConfiguration
{

    [JsonPropertyName("organization")]
    public string? Organization { get; set; }
}

internal class EndpointConfiguration : KeyConfiguration
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
    [JsonRequired]
    public string? Model { get; set; }
}