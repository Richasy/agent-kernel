// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel;

/// <summary>
/// Chat configuration manager.
/// </summary>
public abstract class ChatConfigManagerBase : IChatConfigManager
{
    /// <inheritdoc/>
    public ChatClientConfiguration? Configuration { get; private set; }

    /// <inheritdoc/>
    public async Task<ChatClientConfigBase?> GetChatConfigAsync(ChatProviderType provider)
    {
        await InitializeAsync().ConfigureAwait(false);
        return provider switch
        {
            ChatProviderType.Ollama => Configuration?.Ollama,
            ChatProviderType.OpenAI => Configuration?.OpenAI,
            ChatProviderType.AzureOpenAI => Configuration?.AzureOpenAI,
            ChatProviderType.AzureAI => Configuration?.AzureAI,
            ChatProviderType.Gemini => Configuration?.Gemini,
            ChatProviderType.Anthropic => Configuration?.Anthropic,
            ChatProviderType.Moonshot => Configuration?.Moonshot,
            ChatProviderType.ZhiPu => Configuration?.ZhiPu,
            ChatProviderType.LingYi => Configuration?.LingYi,
            ChatProviderType.DeepSeek => Configuration?.DeepSeek,
            ChatProviderType.Qwen => Configuration?.Qwen,
            ChatProviderType.Ernie => Configuration?.Ernie,
            ChatProviderType.Hunyuan => Configuration?.Hunyuan,
            ChatProviderType.Spark => Configuration?.Spark,
            ChatProviderType.OpenRouter => Configuration?.OpenRouter,
            ChatProviderType.TogetherAI => Configuration?.TogetherAI,
            ChatProviderType.Groq => Configuration?.Groq,
            ChatProviderType.Perplexity => Configuration?.Perplexity,
            ChatProviderType.Mistral => Configuration?.Mistral,
            ChatProviderType.SiliconFlow => Configuration?.SiliconFlow,
            ChatProviderType.Doubao => Configuration?.Doubao,
            ChatProviderType.XAI => Configuration?.XAI,
            ChatProviderType.Onnx => Configuration?.Onnx,
            ChatProviderType.Windows => null,
            _ => throw new NotImplementedException(),
        };
    }

    /// <inheritdoc/>
    public async Task<AIServiceConfig?> GetServiceConfigAsync(ChatProviderType provider, ChatModel model)
    {
        var config = await GetChatConfigAsync(provider).ConfigureAwait(false);
        var aiConfig = ConvertToConfig(config);
        if (aiConfig is not null)
        {
            aiConfig.Model = model.Id;
        }

        return aiConfig;
    }

    /// <inheritdoc/>
    public async Task SaveChatConfigAsync(Dictionary<ChatProviderType, ChatClientConfigBase> configMap)
    {
        await InitializeAsync().ConfigureAwait(false);
        if (Configuration is null)
        {
            throw new InvalidOperationException("Configuration is not initialized.");
        }

        foreach (var item in configMap)
        {
            switch (item.Key)
            {
                case ChatProviderType.Ollama:
                    Configuration.Ollama = item.Value as OllamaChatConfig;
                    break;
                case ChatProviderType.OpenAI:
                    Configuration.OpenAI = item.Value as OpenAIChatConfig;
                    break;
                case ChatProviderType.AzureOpenAI:
                    Configuration.AzureOpenAI = item.Value as AzureOpenAIChatConfig;
                    break;
                case ChatProviderType.AzureAI:
                    Configuration.AzureAI = item.Value as AzureAIChatConfig;
                    break;
                case ChatProviderType.Gemini:
                    Configuration.Gemini = item.Value as GeminiChatConfig;
                    break;
                case ChatProviderType.Anthropic:
                    Configuration.Anthropic = item.Value as AnthropicChatConfig;
                    break;
                case ChatProviderType.Moonshot:
                    Configuration.Moonshot = item.Value as MoonshotChatConfig;
                    break;
                case ChatProviderType.ZhiPu:
                    Configuration.ZhiPu = item.Value as ZhiPuChatConfig;
                    break;
                case ChatProviderType.LingYi:
                    Configuration.LingYi = item.Value as LingYiChatConfig;
                    break;
                case ChatProviderType.DeepSeek:
                    Configuration.DeepSeek = item.Value as DeepSeekChatConfig;
                    break;
                case ChatProviderType.Qwen:
                    Configuration.Qwen = item.Value as QwenChatConfig;
                    break;
                case ChatProviderType.Ernie:
                    Configuration.Ernie = item.Value as ErnieChatConfig;
                    break;
                case ChatProviderType.Hunyuan:
                    Configuration.Hunyuan = item.Value as HunyuanChatConfig;
                    break;
                case ChatProviderType.Spark:
                    Configuration.Spark = item.Value as SparkChatConfig;
                    break;
                case ChatProviderType.OpenRouter:
                    Configuration.OpenRouter = item.Value as OpenRouterChatConfig;
                    break;
                case ChatProviderType.TogetherAI:
                    Configuration.TogetherAI = item.Value as TogetherAIChatConfig;
                    break;
                case ChatProviderType.Groq:
                    Configuration.Groq = item.Value as GroqChatConfig;
                    break;
                case ChatProviderType.Perplexity:
                    Configuration.Perplexity = item.Value as PerplexityChatConfig;
                    break;
                case ChatProviderType.Mistral:
                    Configuration.Mistral = item.Value as MistralChatConfig;
                    break;
                case ChatProviderType.SiliconFlow:
                    Configuration.SiliconFlow = item.Value as SiliconFlowChatConfig;
                    break;
                case ChatProviderType.Doubao:
                    Configuration.Doubao = item.Value as DoubaoChatConfig;
                    break;
                case ChatProviderType.XAI:
                    Configuration.XAI = item.Value as XAIChatConfig;
                    break;
                case ChatProviderType.Onnx:
                    Configuration.Onnx = item.Value as OnnxChatConfig;
                    break;
                case ChatProviderType.Windows:
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        await OnSaveAsync(Configuration).ConfigureAwait(false);
    }

    /// <summary>
    /// 转换为配置.
    /// </summary>
    protected abstract AIServiceConfig? ConvertToConfig(ChatClientConfigBase? config);

    /// <summary>
    /// Initialize the configuration.
    /// </summary>
    /// <returns><see cref="ChatClientConfiguration"/>.</returns>
    protected abstract Task<ChatClientConfiguration> OnInitializeAsync();

    /// <summary>
    /// Save the configuration.
    /// </summary>
    protected abstract Task OnSaveAsync(ChatClientConfiguration configuration);

    private async Task InitializeAsync()
    {
        if (Configuration is not null)
        {
            return;
        }

        Configuration = await OnInitializeAsync().ConfigureAwait(false);
    }
}
