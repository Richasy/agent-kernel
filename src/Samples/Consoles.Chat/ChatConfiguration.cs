// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Consoles.Chat;

internal sealed class ChatConfiguration
{
    [JsonPropertyName("azure_openai")]
    public AzureOpenAIConfiguration? AzureOpenAI { get; set; }

    [JsonPropertyName("xai")]
    public XAIConfiguration? XAI { get; set; }

    [JsonPropertyName("openai")]
    public OpenAIConfiguration? OpenAI { get; set; }

    [JsonPropertyName("zhipu")]
    public ZhiPuConfiguration? ZhiPu { get; set; }

    [JsonPropertyName("lingyi")]
    public LingYiConfiguration? LingYi { get; set; }

    [JsonPropertyName("anthropic")]
    public AnthropicConfiguration? Anthropic { get; set; }

    [JsonPropertyName("moonshot")]
    public MoonshotConfiguration? Moonshot { get; set; }

    [JsonPropertyName("gemini")]
    public GeminiConfiguration? Gemini { get; set; }

    [JsonPropertyName("deepseek")]
    public DeepSeekConfiguration? DeepSeek { get; set; }

    [JsonPropertyName("qwen")]
    public QwenConfiguration? Qwen { get; set; }

    [JsonPropertyName("ernie")]
    public ErnieConfiguration? Ernie { get; set; }

    [JsonPropertyName("hunyuan")]
    public HunyuanConfiguration? Hunyuan { get; set; }
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
    public string? Endpoint { get; set; }

    [JsonPropertyName("organization")]
    public string? Organization { get; set; }
}

internal sealed class ZhiPuConfiguration
{
    [JsonPropertyName("key")]
    [JsonRequired]
    public string? AccessKey { get; set; }

    [JsonPropertyName("model")]
    [JsonRequired]
    public string? Model { get; set; }
}

internal sealed class LingYiConfiguration
{
    [JsonPropertyName("key")]
    [JsonRequired]
    public string? AccessKey { get; set; }

    [JsonPropertyName("model")]
    [JsonRequired]
    public string? Model { get; set; }
}

internal sealed class AnthropicConfiguration
{
    [JsonPropertyName("key")]
    [JsonRequired]
    public string? AccessKey { get; set; }

    [JsonPropertyName("model")]
    [JsonRequired]
    public string? Model { get; set; }

    [JsonPropertyName("endpoint")]
    public string? Endpoint { get; set; }
}

internal sealed class MoonshotConfiguration
{
    [JsonPropertyName("key")]
    [JsonRequired]
    public string? AccessKey { get; set; }

    [JsonPropertyName("model")]
    [JsonRequired]
    public string? Model { get; set; }
}

internal sealed class GeminiConfiguration
{
    [JsonPropertyName("key")]
    [JsonRequired]
    public string? AccessKey { get; set; }

    [JsonPropertyName("model")]
    [JsonRequired]
    public string? Model { get; set; }

    [JsonPropertyName("endpoint")]
    public string? Endpoint { get; set; }
}

internal sealed class DeepSeekConfiguration
{
    [JsonPropertyName("key")]
    [JsonRequired]
    public string? AccessKey { get; set; }

    [JsonPropertyName("model")]
    [JsonRequired]
    public string? Model { get; set; }
}

internal sealed class QwenConfiguration
{
    [JsonPropertyName("key")]
    [JsonRequired]
    public string? AccessKey { get; set; }

    [JsonPropertyName("model")]
    [JsonRequired]
    public string? Model { get; set; }
}

internal sealed class ErnieConfiguration
{
    [JsonPropertyName("key")]
    [JsonRequired]
    public string? AccessKey { get; set; }

    [JsonPropertyName("model")]
    [JsonRequired]
    public string? Model { get; set; }
}

internal sealed class HunyuanConfiguration
{
    [JsonPropertyName("key")]
    [JsonRequired]
    public string? AccessKey { get; set; }

    [JsonPropertyName("model")]
    [JsonRequired]
    public string? Model { get; set; }
}