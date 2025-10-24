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
            new("claude-sonnet-4-5", "Claude Sonnet 4.5", toolSupport: true, visionSupport: true),
            new("claude-opus-4-1", "Claude Opus 4.1", toolSupport: true, visionSupport: true),
            new("claude-opus-4", "Claude Opus 4", toolSupport: true, visionSupport: true),
            new("claude-sonnet-4", "Claude Sonnet 4", toolSupport: true, visionSupport: true),
            new("claude-3-7-sonnet-latest", "Claude Sonnet 3.7", toolSupport: true, visionSupport: true),
            new("claude-3-5-haiku-latest", "Claude Haiku 3.5", toolSupport: true, visionSupport: true),
        ];
    }
}
