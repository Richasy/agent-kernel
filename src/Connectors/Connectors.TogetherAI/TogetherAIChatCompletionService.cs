// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using OpenAI;
using Richasy.AgentKernel.ChatCompletion;
using Richasy.AgentKernel.Connectors.TogetherAI.Models;

namespace Richasy.AgentKernel.Connectors.TogetherAI;

/// <summary>
/// Together.AI Chat Completion Service.
/// </summary>
public sealed class TogetherAIChatCompletionService : IChatCompletionService
{
    private TogetherAIServiceConfig? _config;

    /// <inheritdoc/>
    public IChatClient? Client
    {
        get => field ?? throw new InvalidOperationException("The service has not been initialized.");
        set;
    }

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig config)
    {
        if (config is not TogetherAIServiceConfig taiConfig)
        {
            throw new ArgumentException("The configuration is not valid.", nameof(config));
        }

        if (_config != null && taiConfig.Equals(_config))
        {
            return;
        }

        _config = taiConfig;
        var coreClient = new OpenAIClient(new(_config.AccessKey), new OpenAIClientOptions
        {
            Endpoint = new Uri("https://api.together.xyz/v1"),
        });

        Client = coreClient.AsChatClient(_config.Model);
    }
}
