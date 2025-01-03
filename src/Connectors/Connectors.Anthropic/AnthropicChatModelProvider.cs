// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.ChatCompletion;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Anthropic;

/// <summary>
/// Provides chat models for the Anthropic connector.
/// </summary>
public sealed class AnthropicChatModelProvider : IChatModelProvider
{
    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetModels()
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
