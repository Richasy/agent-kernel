// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Core.OpenAI;
using Richasy.AgentKernel.Extensions.AI;
using Microsoft.Extensions.AI;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.Perplexity.Models;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Perplexity;

/// <summary>
/// Perplexity Chat Completion Service.
/// </summary>
public sealed class PerplexityChatService : IChatService
{
    private PerplexityServiceConfig? _config;

    /// <inheritdoc/>
    public IChatClient? Client { get; set; }

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

        Client?.Dispose();
        Client = coreClient.AsChatClient(_config.Model!);
    }

    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetPredefinedModels() =>
    [
        new("llama-3.1-sonar-small-128k-online", "Llama 3.1 Sonar Small 128K Online"),
        new("llama-3.1-sonar-large-128k-online", "Llama 3.1 Sonar Large 128K Online"),
        new("llama-3.1-sonar-huge-128k-online", "Llama 3.1 Sonar Huge 128K Online"),
    ];
}
