// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Consoles.Chat;

internal sealed class ChatConfiguration
{
    [JsonPropertyName("azure_openai")]
    public EndpointConfiguration? AzureOpenAI { get; set; }

    [JsonPropertyName("azure_ai")]
    public EndpointConfiguration? AzureAI { get; set; }

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
    public ErnieConfiguration? Ernie { get; set; }

    [JsonPropertyName("hunyuan")]
    public KeyConfiguration? Hunyuan { get; set; }

    [JsonPropertyName("spark")]
    public KeyConfiguration? Spark { get; set; }

    [JsonPropertyName("doubao")]
    public KeyConfiguration? Doubao { get; set; }

    [JsonPropertyName("siliconflow")]
    public KeyConfiguration? SiliconFlow { get; set; }

    [JsonPropertyName("openrouter")]
    public KeyConfiguration? OpenRouter { get; set; }

    [JsonPropertyName("togetherai")]
    public KeyConfiguration? TogetherAI { get; set; }

    [JsonPropertyName("groq")]
    public KeyConfiguration? Groq { get; set; }

    [JsonPropertyName("mistral")]
    public MistralConfiguration? Mistral { get; set; }

    [JsonPropertyName("ollama")]
    public OllamaConfiguration? Ollama { get; set; }
}

internal sealed class OpenAIConfiguration : EndpointConfiguration
{

    [JsonPropertyName("organization")]
    public string? Organization { get; set; }
}

internal sealed class OllamaConfiguration
{
    [JsonPropertyName("model")]
    public required string Model { get; set; }

    [JsonPropertyName("endpoint")]
    public required string Endpoint { get; set; }
}

internal sealed class ErnieConfiguration : KeyConfiguration
{
    [JsonPropertyName("secret")]
    public required string SecretKey { get; set; }
}

internal class EndpointConfiguration : KeyConfiguration
{
    [JsonPropertyName("endpoint")]
    public string? Endpoint { get; set; }
}

internal sealed class MistralConfiguration : KeyConfiguration
{
    [JsonPropertyName("use_codestral")]
    public bool UseCodestralApi { get; set; }
}

internal class KeyConfiguration
{
    [JsonPropertyName("key")]
    [JsonRequired]
    public string? AccessKey { get; set; }

    [JsonPropertyName("model")]
    public string? Model { get; set; }
}