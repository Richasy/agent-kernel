// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.LingYi;

/// <summary>
/// Provides chat models for the LingYi connector.
/// </summary>
public sealed class LingYiChatModelProvider : IChatModelProvider
{
    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetModels() =>
    [
        new("yi-lightning", "Yi Lightning"),
        new("yi-medium-200k", "Yi Medium 200K"),
        new("yi-large", "Yi Large"),
        new("yi-vision", "Yi Vision", visionSupport: true),
        new("yi-vision-v2", "Yi Vision V2", visionSupport: true),
    ];
}
