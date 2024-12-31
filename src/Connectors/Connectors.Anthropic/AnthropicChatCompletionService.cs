// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Anthropic;
using Microsoft.Extensions.AI;
using Richasy.AgentKernel.ChatCompletion;
using Richasy.AgentKernel.Connectors.Anthropic.Models;

namespace Richasy.AgentKernel.Connectors.Anthropic;

/// <summary>
/// Anthropic Chat Completion Service.
/// </summary>
public sealed class AnthropicChatCompletionService : IChatCompletionService
{
    private AnthropicServiceConfig? _config;

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
        if (config is not AnthropicServiceConfig anthropicConfig)
        {
            throw new ArgumentException("The configuration is not valid.", nameof(config));
        }

        if (_config != null && anthropicConfig.Equals(_config))
        {
            return;
        }

        _config = anthropicConfig;
        Client = new AnthropicClient(_config.AccessKey, baseUri: _config.Endpoint);
    }
}
