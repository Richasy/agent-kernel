// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Anthropic;
using Microsoft.Extensions.AI;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.Anthropic.Models;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Anthropic;

/// <summary>
/// Anthropic Chat Completion Service.
/// </summary>
public sealed class AnthropicChatService : IChatService
{
    private AnthropicServiceConfig? _config;

    /// <inheritdoc/>
    public IChatClient? Client { get; set; }

    /// <inheritdoc/>
    public AIServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig? config)
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
        Client?.Dispose();
        Client = new AnthropicClient(_config.AccessKey, baseUri: _config.Endpoint);
    }

    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetPredefinedModels()
    {
        return
        [
            new("claude-3-7-sonnet-latest", "Claude 3.7 Sonnet", toolSupport: true, visionSupport: true),
            new("claude-3-5-haiku-latest", "Claude 3.5 Haiku", toolSupport: true, visionSupport: true),
            new("claude-3-opus-latest", "Claude 3 Opus", toolSupport: true, visionSupport: true),
        ];
    }
}
