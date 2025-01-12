// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.XAI;

/// <summary>
/// Provides chat models for the XAI connector.
/// </summary>
public sealed class XAIChatModelProvider : IChatModelProvider
{
    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetModels() =>
    [
        new("grok-2-latest", "Grok 2", toolSupport: true),
        new("grok-beta", "Grok Beta", toolSupport: true),
    ];
}
