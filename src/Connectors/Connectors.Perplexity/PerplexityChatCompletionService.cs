// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using OpenAI;
using Richasy.AgentKernel.ChatCompletion;
using Richasy.AgentKernel.Connectors.Perplexity.Models;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Perplexity;

/// <summary>
/// Perplexity Chat Completion Service.
/// </summary>
public sealed class PerplexityChatCompletionService : IChatCompletionService
{
    private PerplexityServiceConfig? _config;

    /// <inheritdoc/>
    public IChatClient? Client
    {
        get => field ?? throw new InvalidOperationException("The service has not been initialized.");
        set;
    }

    /// <inheritdoc/>
    public AIServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig config)
    {
        if (config is not PerplexityServiceConfig plexConfig)
        {
            throw new ArgumentException("The configuration is not valid.", nameof(config));
        }

        if (_config != null && plexConfig.Equals(_config))
        {
            return;
        }

        _config = plexConfig;
        var coreClient = new OpenAIClient(new(_config.AccessKey), new OpenAIClientOptions
        {
            Endpoint = new Uri("https://api.perplexity.ai"),
        });

        Client = coreClient.AsChatClient(_config.Model!);
    }
}
