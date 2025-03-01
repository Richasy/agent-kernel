// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;
using System.Text.Json;

namespace Richasy.AgentKernel;

/// <summary>
/// 服务类型.
/// </summary>
[JsonConverter(typeof(ChatProviderTypeConverter))]
public enum ChatProviderType
{
    /// <summary>
    /// Open AI.
    /// </summary>
    OpenAI,

    /// <summary>
    /// Azure Open AI.
    /// </summary>
    AzureOpenAI,

    /// <summary>
    /// Azure AI.
    /// </summary>
    AzureAI,

    /// <summary>
    /// Google Gemini.
    /// </summary>
    Gemini,

    /// <summary>
    /// Anthropic.
    /// </summary>
    Anthropic,

    /// <summary>
    /// 月之暗面.
    /// </summary>
    Moonshot,

    /// <summary>
    /// 智谱 AI.
    /// </summary>
    ZhiPu,

    /// <summary>
    /// 零一万物.
    /// </summary>
    LingYi,

    /// <summary>
    /// DeepSeek.
    /// </summary>
    DeepSeek,

    /// <summary>
    /// 通义千问.
    /// </summary>
    Qwen,

    /// <summary>
    /// 文心一言.
    /// </summary>
    Ernie,

    /// <summary>
    /// 腾讯混元.
    /// </summary>
    Hunyuan,

    /// <summary>
    /// 豆包.
    /// </summary>
    Doubao,

    /// <summary>
    /// 讯飞星火.
    /// </summary>
    Spark,

    /// <summary>
    /// Open Router.
    /// </summary>
    OpenRouter,

    /// <summary>
    /// Together AI.
    /// </summary>
    TogetherAI,

    /// <summary>
    /// Groq.
    /// </summary>
    Groq,

    /// <summary>
    /// Perplexity.
    /// </summary>
    Perplexity,

    /// <summary>
    /// Mistral AI.
    /// </summary>
    Mistral,

    /// <summary>
    /// Silicon Flow.
    /// </summary>
    SiliconFlow,

    /// <summary>
    /// Ollama.
    /// </summary>
    Ollama,

    /// <summary>
    /// xAI.
    /// </summary>
    XAI,

    /// <summary>
    /// ONNX.
    /// </summary>
    Onnx,

    /// <summary>
    /// Windows.
    /// </summary>
    Windows,
}

/// <summary>
/// 服务类型转换器.
/// </summary>
public sealed class ChatProviderTypeConverter : JsonConverter<ChatProviderType>
{
    /// <inheritdoc/>
    public override ChatProviderType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString()!.ToLower(System.Globalization.CultureInfo.CurrentCulture) switch
        {
            "openai" => ChatProviderType.OpenAI,
            "azure_openai" or "azureopenai" => ChatProviderType.AzureOpenAI,
            "azure_ai" or "azureai" => ChatProviderType.AzureAI,
            "gemini" => ChatProviderType.Gemini,
            "anthropic" => ChatProviderType.Anthropic,
            "open_router" => ChatProviderType.OpenRouter,
            "together_ai" or "togetherai" => ChatProviderType.TogetherAI,
            "groq" => ChatProviderType.Groq,
            "perplexity" => ChatProviderType.Perplexity,
            "mistral" => ChatProviderType.Mistral,
            "moonshot" => ChatProviderType.Moonshot,
            "zhipu" => ChatProviderType.ZhiPu,
            "lingyi" => ChatProviderType.LingYi,
            "qwen" => ChatProviderType.Qwen,
            "ernie" => ChatProviderType.Ernie,
            "spark" => ChatProviderType.Spark,
            "deep_seek" or "deepseek" => ChatProviderType.DeepSeek,
            "hunyuan" => ChatProviderType.Hunyuan,
            "ollama" => ChatProviderType.Ollama,
            "silicon_flow" or "siliconflow" => ChatProviderType.SiliconFlow,
            "doubao" => ChatProviderType.Doubao,
            "xai" => ChatProviderType.XAI,
            "onnx" => ChatProviderType.Onnx,
            "windows" => ChatProviderType.Windows,
            _ => throw new JsonException(),
        };
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, ChatProviderType value, JsonSerializerOptions options)
    {
        var text = value switch
        {
            ChatProviderType.OpenAI => "openai",
            ChatProviderType.AzureOpenAI => "azure_openai",
            ChatProviderType.AzureAI => "azure_ai",
            ChatProviderType.Gemini => "gemini",
            ChatProviderType.Anthropic => "anthropic",
            ChatProviderType.OpenRouter => "open_router",
            ChatProviderType.TogetherAI => "together_ai",
            ChatProviderType.Groq => "groq",
            ChatProviderType.Perplexity => "perplexity",
            ChatProviderType.Mistral => "mistral",
            ChatProviderType.Moonshot => "moonshot",
            ChatProviderType.ZhiPu => "zhipu",
            ChatProviderType.LingYi => "lingyi",
            ChatProviderType.Qwen => "qwen",
            ChatProviderType.Ernie => "ernie",
            ChatProviderType.Spark => "spark",
            ChatProviderType.DeepSeek => "deepseek",
            ChatProviderType.Hunyuan => "hunyuan",
            ChatProviderType.Ollama => "ollama",
            ChatProviderType.SiliconFlow => "siliconflow",
            ChatProviderType.Doubao => "doubao",
            ChatProviderType.XAI => "xai",
            ChatProviderType.Onnx => "onnx",
            ChatProviderType.Windows => "windows",
            _ => throw new JsonException(),
        };

        writer.WriteStringValue(text);
    }
}
