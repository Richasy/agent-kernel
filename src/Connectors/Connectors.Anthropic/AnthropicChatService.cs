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
            new("claude-3-5-sonnet-20241022", "Claude 3.5 Sonnet", toolSupport: true, visionSupport: true),
            new("claude-3-5-haiku-20241022", "Claude 3.5 Haiku", toolSupport: true, visionSupport: true),
            new("claude-3-haiku-20240307", "Claude 3 Haiku", toolSupport: true, visionSupport: true),
            new("claude-3-opus-20240229", "Claude 3 Opus", toolSupport: true, visionSupport: true),
            new("claude-3-sonnet-20240229", "Claude 3 Sonnet", toolSupport: true, visionSupport: true),
            new("claude-2.1", "Claude 2.1"),
            new("claude-2.0", "Claude 2.0"),
        ];
    }
}
