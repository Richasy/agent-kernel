// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Connectors.DeepSeek.Models;
using Richasy.AgentKernel;
using Richasy.AgentKernel.Connectors.Ali.Models;
using Richasy.AgentKernel.Connectors.Anthropic.Models;
using Richasy.AgentKernel.Connectors.Azure.Models;
using Richasy.AgentKernel.Connectors.Google.Models;
using Richasy.AgentKernel.Connectors.Groq.Models;
using Richasy.AgentKernel.Connectors.IFlyTek.Models;
using Richasy.AgentKernel.Connectors.LingYi.Models;
using Richasy.AgentKernel.Connectors.Moonshot.Models;
using Richasy.AgentKernel.Connectors.OpenRouter.Models;
using Richasy.AgentKernel.Connectors.SiliconFlow.Models;
using Richasy.AgentKernel.Connectors.Tencent.Models;
using Richasy.AgentKernel.Connectors.TogetherAI.Models;
using Richasy.AgentKernel.Connectors.Volcano.Models;
using Richasy.AgentKernel.Connectors.XAI.Models;
using Richasy.AgentKernel.Connectors.ZhiPu.Models;
using Richasy.AgentKernel.Models;
using System.Text.Json;

namespace Consoles.Chat;

/// <summary>
/// Chat configuration manager.
/// </summary>
internal sealed class ChatConfigManager : ChatConfigManagerBase
{
    /// <inheritdoc/>
    protected override async Task<ChatClientConfiguration> OnInitializeAsync()
    {
        var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "env.json");
        if (!File.Exists(configPath))
        {
            throw new FileNotFoundException("Config file not found.");
        }

        var configContent = await File.ReadAllTextAsync(configPath);
        return JsonSerializer.Deserialize(configContent, JsonGenerationContext.Default.ChatClientConfiguration)!;
    }

    /// <inheritdoc/>
    protected override Task OnSaveAsync(ChatClientConfiguration configuration) => Task.CompletedTask;

    protected override AIServiceConfig? ConvertToConfig(ChatClientConfigBase? config)
    {
        return config switch
        {
            OpenAIChatConfig openAIConfig => openAIConfig.ToAIServiceConfig(),
            AzureOpenAIChatConfig azureOaiConfig => azureOaiConfig.ToAIServiceConfig<AzureOpenAIServiceConfig>(),
            AzureAIChatConfig azureConfig => azureConfig.ToAIServiceConfig<AzureOpenAIServiceConfig>(),
            XAIChatConfig xaiConfig => xaiConfig.ToAIServiceConfig<XAIServiceConfig>(),
            ZhiPuChatConfig zhiPuConfig => zhiPuConfig.ToAIServiceConfig<ZhiPuServiceConfig>(),
            LingYiChatConfig lingYiConfig => lingYiConfig.ToAIServiceConfig<LingYiServiceConfig>(),
            AnthropicChatConfig anthropicConfig => anthropicConfig.ToAIServiceConfig<AnthropicServiceConfig>(),
            MoonshotChatConfig moonshotConfig => moonshotConfig.ToAIServiceConfig<MoonshotServiceConfig>(),
            GeminiChatConfig geminiConfig => geminiConfig.ToAIServiceConfig<GeminiServiceConfig>(),
            DeepSeekChatConfig deepSeekConfig => deepSeekConfig.ToAIServiceConfig<DeepSeekServiceConfig>(),
            QwenChatConfig qwenConfig => qwenConfig.ToAIServiceConfig<QwenServiceConfig>(),
            ErnieChatConfig ernieConfig => ernieConfig.ToAIServiceConfig(),
            HunyuanChatConfig hunyuanConfig => hunyuanConfig.ToAIServiceConfig<HunyuanChatServiceConfig>(),
            SparkChatConfig sparkConfig => sparkConfig.ToAIServiceConfig<SparkChatServiceConfig>(),
            DoubaoChatConfig douBaoConfig => douBaoConfig.ToAIServiceConfig<DoubaoServiceConfig>(),
            SiliconFlowChatConfig siliconFlowConfig => siliconFlowConfig.ToAIServiceConfig<SiliconFlowServiceConfig>(),
            OpenRouterChatConfig openRouterConfig => openRouterConfig.ToAIServiceConfig<OpenRouterServiceConfig>(),
            TogetherAIChatConfig togetherAIConfig => togetherAIConfig.ToAIServiceConfig<TogetherAIServiceConfig>(),
            GroqChatConfig groqConfig => groqConfig.ToAIServiceConfig<GroqServiceConfig>(),
            MistralChatConfig mistralConfig => mistralConfig.ToAIServiceConfig(),
            OllamaChatConfig ollamaConfig => ollamaConfig.ToAIServiceConfig(),
            _ => default,
        };
    }
}
