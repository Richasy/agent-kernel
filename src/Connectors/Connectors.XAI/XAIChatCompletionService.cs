// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using OpenAI;
using Richasy.AgentKernel.ChatCompletion;
using Richasy.AgentKernel.Connectors.XAI.Models;

namespace Richasy.AgentKernel.Connectors.XAI;

/// <summary>
/// XAI Chat Completion Service.
/// </summary>
public sealed class XAIChatCompletionService : IChatCompletionService
{
    private XAIServiceConfig? _config;

    /// <inheritdoc/>
    public IChatClient? Client
    {
        get => field ?? throw new InvalidOperationException("The service has not been initialized.");
        set;
    }

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig config)
    {
        if (config is not XAIServiceConfig xaiConfig)
        {
            throw new ArgumentException("The configuration is not valid.", nameof(config));
        }

        if (_config != null && xaiConfig.Equals(_config))
        {
            return;
        }

        _config = xaiConfig;
        var coreClient = new OpenAIClient(new(_config.AccessKey), new OpenAIClientOptions
        {
            Endpoint = new Uri("https://api.x.ai/v1"),
        });

        Client = coreClient.AsChatClient(_config.Model);
    }
}
