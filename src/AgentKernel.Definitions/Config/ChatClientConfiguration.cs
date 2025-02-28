// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel;

/// <summary>
/// 聊天客户端配置.
/// </summary>
public sealed class ChatClientConfiguration
{
    /// <summary>
    /// Open AI 客户端配置.
    /// </summary>
    [JsonPropertyName("openai")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public OpenAIChatConfig? OpenAI { get; set; }

    /// <summary>
    /// Azure Open AI 客户端配置.
    /// </summary>
    [JsonPropertyName("azure_openai")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AzureOpenAIChatConfig? AzureOpenAI { get; set; }

    /// <summary>
    /// Azure AI 客户端配置.
    /// </summary>
    [JsonPropertyName("azure_ai")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AzureAIChatConfig? AzureAI { get; set; }

    /// <summary>
    /// 智谱客户端配置.
    /// </summary>
    [JsonPropertyName("zhipu")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ZhiPuChatConfig? ZhiPu { get; set; }

    /// <summary>
    /// 零一万物客户端配置.
    /// </summary>
    [JsonPropertyName("lingyi")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public LingYiChatConfig? LingYi { get; set; }

    /// <summary>
    /// 月之暗面客户端配置.
    /// </summary>
    [JsonPropertyName("moonshot")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MoonshotChatConfig? Moonshot { get; set; }

    /// <summary>
    /// 通义千问客户端配置.
    /// </summary>
    [JsonPropertyName("qwen")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public QwenChatConfig? Qwen { get; set; }

    /// <summary>
    /// DeepSeek 客户端配置.
    /// </summary>
    [JsonPropertyName("deepseek")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DeepSeekChatConfig? DeepSeek { get; set; }

    /// <summary>
    /// 千帆客户端配置.
    /// </summary>
    [JsonPropertyName("ernie")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ErnieChatConfig? Ernie { get; set; }

    /// <summary>
    /// 讯飞星火客户端配置.
    /// </summary>
    [JsonPropertyName("spark")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SparkChatConfig? Spark { get; set; }

    /// <summary>
    /// Gemini 客户端配置.
    /// </summary>
    [JsonPropertyName("gemini")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public GeminiChatConfig? Gemini { get; set; }

    /// <summary>
    /// Groq 客户端配置.
    /// </summary>
    [JsonPropertyName("groq")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public GroqChatConfig? Groq { get; set; }

    /// <summary>
    /// Mistral AI 客户端配置.
    /// </summary>
    [JsonPropertyName("mistral")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MistralChatConfig? Mistral { get; set; }

    /// <summary>
    /// Perplexity 客户端配置.
    /// </summary>
    [JsonPropertyName("perplexity")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PerplexityChatConfig? Perplexity { get; set; }

    /// <summary>
    /// Together AI 客户端配置.
    /// </summary>
    [JsonPropertyName("together_ai")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TogetherAIChatConfig? TogetherAI { get; set; }

    /// <summary>
    /// Open Router 客户端配置.
    /// </summary>
    [JsonPropertyName("open_router")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public OpenRouterChatConfig? OpenRouter { get; set; }

    /// <summary>
    /// Anthropic 客户端配置.
    /// </summary>
    [JsonPropertyName("anthropic")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AnthropicChatConfig? Anthropic { get; set; }

    /// <summary>
    /// Ollama 客户端配置.
    /// </summary>
    [JsonPropertyName("ollama")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public OllamaChatConfig? Ollama { get; set; }

    /// <summary>
    /// 混元客户端配置.
    /// </summary>
    [JsonPropertyName("hunyuan")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public HunyuanChatConfig? Hunyuan { get; set; }

    /// <summary>
    /// 硅动客户端配置.
    /// </summary>
    [JsonPropertyName("siliconflow")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SiliconFlowChatConfig? SiliconFlow { get; set; }

    /// <summary>
    /// 豆包客户端配置.
    /// </summary>
    [JsonPropertyName("doubao")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DoubaoChatConfig? Doubao { get; set; }

    /// <summary>
    /// XAI 客户端配置.
    /// </summary>
    [JsonPropertyName("xai")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public XAIChatConfig? XAI { get; set; }

    /// <summary>
    /// ONNX 客户端配置.
    /// </summary>
    [JsonPropertyName("onnx")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public OnnxChatConfig? Onnx { get; set; }
}

/// <summary>
/// Open AI 客户端配置.
/// </summary>
public class OpenAIChatConfig : ChatEndpointConfigBase
{
    /// <summary>
    /// 组织 ID.
    /// </summary>
    [JsonPropertyName("organization")]
    public string? OrganizationId { get; set; }
}

/// <summary>
/// Azure Open AI 客户端配置.
/// </summary>
public class AzureOpenAIChatConfig : ChatEndpointConfigBase
{
    /// <inheritdoc/>
    public override bool IsValid()
    {
        return base.IsValid()
            && !string.IsNullOrEmpty(Endpoint)
            && CustomModels?.Count > 0;
    }
}

/// <summary>
/// Azure AI 客户端配置.
/// </summary>
public class AzureAIChatConfig : ChatEndpointConfigBase
{
    /// <inheritdoc/>
    public override bool IsValid()
    {
        return base.IsValid()
            && !string.IsNullOrEmpty(Endpoint);
    }
}

/// <summary>
/// 千帆客户端配置.
/// </summary>
public class ErnieChatConfig : ChatClientConfigBase
{
    /// <summary>
    /// 密匙.
    /// </summary>
    [JsonPropertyName("secret")]
    public string? Secret { get; set; }

    /// <inheritdoc/>
    public override bool IsValid()
        => base.IsValid() && !string.IsNullOrEmpty(Secret);
}

/// <summary>
/// 讯飞星火服务配置.
/// </summary>
public sealed class SparkChatConfig : ChatClientConfigBase;

/// <summary>
/// 混元客户端配置.
/// </summary>
public sealed class HunyuanChatConfig : ChatClientConfigBase;

/// <summary>
/// 豆包大模型配置.
/// </summary>
public sealed class DoubaoChatConfig : ChatClientConfigBase
{
    /// <inheritdoc/>
    public override bool IsValid()
        => base.IsValid() && IsCustomModelNotEmpty();
}

/// <summary>
/// xAI大模型配置.
/// </summary>
public sealed class XAIChatConfig : ChatClientConfigBase;

/// <summary>
/// 智谱客户端配置.
/// </summary>
public sealed class ZhiPuChatConfig : ChatClientConfigBase;

/// <summary>
/// 零一万物客户端配置.
/// </summary>
public sealed class LingYiChatConfig : ChatClientConfigBase;

/// <summary>
/// 月之暗面客户端配置.
/// </summary>
public sealed class MoonshotChatConfig : ChatClientConfigBase;

/// <summary>
/// 通义千问客户端配置.
/// </summary>
public sealed class QwenChatConfig : ChatClientConfigBase;

/// <summary>
/// Gemini 客户端配置.
/// </summary>
public sealed class GeminiChatConfig : ChatEndpointConfigBase;

/// <summary>
/// Groq 客户端配置.
/// </summary>
public sealed class GroqChatConfig : ChatClientConfigBase;

/// <summary>
/// Mistral AI 客户端配置.
/// </summary>
public sealed class MistralChatConfig : ChatClientConfigBase
{
    /// <summary>
    /// Codestral 密钥.
    /// </summary>
    [JsonPropertyName("codestral_key")]
    public string? CodestralKey { get; set; }

    /// <summary>
    /// 是否使用 Codestral.
    /// </summary>
    [JsonPropertyName("use_codestral")]
    public bool UseCodestral { get; set; }

    /// <inheritdoc/>
    public override bool IsValid()
    {
        return UseCodestral ? !string.IsNullOrEmpty(CodestralKey)
            : base.IsValid();
    }
}

/// <summary>
/// Perplexity 客户端配置.
/// </summary>
public sealed class PerplexityChatConfig : ChatClientConfigBase;

/// <summary>
/// Together AI 客户端配置.
/// </summary>
public sealed class TogetherAIChatConfig : ChatClientConfigBase;

/// <summary>
/// Open Router 客户端配置.
/// </summary>
public sealed class OpenRouterChatConfig : ChatClientConfigBase;

/// <summary>
/// DeepSeek 客户端配置.
/// </summary>
public sealed class DeepSeekChatConfig : ChatClientConfigBase;

/// <summary>
/// Anthropic 客户端配置.
/// </summary>
public sealed class AnthropicChatConfig : ChatEndpointConfigBase;

/// <summary>
/// 硅动客户端配置.
/// </summary>
public sealed class SiliconFlowChatConfig : ChatClientConfigBase;

/// <summary>
/// Ollama 客户端配置.
/// </summary>
public sealed class OllamaChatConfig : ChatEndpointConfigBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaChatConfig"/> class.
    /// </summary>
    public OllamaChatConfig() => Key = "ollama";

    /// <inheritdoc/>
    public override bool IsValid()
        => IsCustomModelNotEmpty() && !string.IsNullOrEmpty(Endpoint);
}

/// <summary>
/// ONNX 客户端配置.
/// </summary>
public sealed class OnnxChatConfig : ChatClientConfigBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OnnxChatConfig"/> class.
    /// </summary>
    public OnnxChatConfig() => Key = "onnx";

    /// <inheritdoc/>
    public override bool IsValid()
        => IsCustomModelNotEmpty();
}

/// <summary>
/// 配置基类.
/// </summary>
public abstract class ChatConfigBase : ConfigBase
{
    /// <summary>
    /// 自定义模型列表.
    /// </summary>
    [JsonPropertyName("models")]
    public IList<ChatModel>? CustomModels { get; set; }

    /// <summary>
    /// 自定义模型是否不为空.
    /// </summary>
    /// <returns>是否不为空.</returns>
    public bool IsCustomModelNotEmpty()
        => CustomModels?.Count > 0;
}

/// <summary>
/// 客户端配置基类.
/// </summary>
public abstract class ChatClientConfigBase : ChatConfigBase
{
    /// <summary>
    /// 访问密钥.
    /// </summary>
    [JsonPropertyName("key")]
    public string? Key { get; set; }

    /// <summary>
    /// 是否有效.
    /// </summary>
    /// <returns>配置是否有效.</returns>
    public virtual bool IsValid()
        => !string.IsNullOrEmpty(Key);
}

/// <summary>
/// 客户端终结点配置基类.
/// </summary>
public abstract class ChatEndpointConfigBase : ChatClientConfigBase
{
    /// <summary>
    /// 终结点.
    /// </summary>
    [JsonPropertyName("endpoint")]
    public string? Endpoint { get; set; }
}