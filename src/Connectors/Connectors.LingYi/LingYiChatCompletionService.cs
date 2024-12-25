// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using OpenAI;
using Richasy.AgentKernel.ChatCompletion;
using Richasy.AgentKernel.Connectors.LingYi.Models;

namespace Richasy.AgentKernel.Connectors.LingYi;

/// <summary>
/// LingYi Chat Completion Service.
/// </summary>
public sealed class LingYiChatCompletionService : IChatCompletionService
{
    private LingYiServiceConfig? _config;

    /// <inheritdoc/>
    public IChatClient? Client
    {
        get => field ?? throw new InvalidOperationException("The service has not been initialized.");
        set;
    }

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig config)
    {
        if (config is not LingYiServiceConfig LingYiConfig)
        {
            throw new ArgumentException("The configuration is not valid.", nameof(config));
        }

        if (_config != null && LingYiConfig.Equals(_config))
        {
            return;
        }

        _config = LingYiConfig;
        var coreClient = new OpenAIClient(new(_config.AccessKey), new OpenAIClientOptions
        {
            Endpoint = new Uri("https://api.lingyiwanwu.com/v1"),
        });

        Client = coreClient.AsChatClient(_config.Model);
    }
}
