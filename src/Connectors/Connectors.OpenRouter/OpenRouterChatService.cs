// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Core.OpenAI;
using Richasy.AgentKernel.Extensions.AI;
using Microsoft.Extensions.AI;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.OpenRouter.Models;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.OpenRouter;

/// <summary>
/// OpenRouter Chat Completion Service.
/// </summary>
public sealed class OpenRouterChatService : IChatService
{
    private OpenRouterServiceConfig? _config;

    /// <inheritdoc/>
    public IChatClient? Client { get; set; }

    /// <inheritdoc/>
    public AIServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig config)
    {
        if (config is not OpenRouterServiceConfig orConfig)
        {
            throw new ArgumentException("The configuration is not valid.", nameof(config));
        }

        if (_config != null && orConfig.Equals(_config))
        {
            return;
        }

        _config = orConfig;
        var coreClient = new OpenAIClient(new(_config.AccessKey), new OpenAIClientOptions
        {
            Endpoint = new Uri("https://openrouter.ai/api/v1"),
        });

        Client?.Dispose();
        Client = coreClient.AsChatClient(_config.Model!);
    }

    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetPredefinedModels() =>
    [
        new("deepseek/deepseek-chat", "DeepSeek V3"),
        new("qwen/qvq-72b-preview", "Qwen: QVQ 72B Preview"),
        new("google/gemini-2.0-flash-thinking-exp:free", "Google: Gemini 2.0 Flash Thinking Experimental"),
        new("sao10k/l3.3-euryale-70b", "Sao10K: Llama 3.3 Euryale 70B"),
        new("openai/o1", "OpenAI: o1"),
    ];
}
