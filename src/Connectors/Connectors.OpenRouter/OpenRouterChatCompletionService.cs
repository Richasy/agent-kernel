// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using OpenAI;
using Richasy.AgentKernel.ChatCompletion;
using Richasy.AgentKernel.Connectors.OpenRouter.Models;

namespace Richasy.AgentKernel.Connectors.OpenRouter;

/// <summary>
/// OpenRouter Chat Completion Service.
/// </summary>
public sealed class OpenRouterChatCompletionService : IChatCompletionService
{
    private OpenRouterServiceConfig? _config;

    /// <inheritdoc/>
    public IChatClient? Client
    {
        get => field ?? throw new InvalidOperationException("The service has not been initialized.");
        set;
    }

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

        Client = coreClient.AsChatClient(_config.Model);
    }
}
