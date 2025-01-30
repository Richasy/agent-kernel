// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.DeepSeek;

/// <summary>
/// Provides chat models for the DeepSeek connector.
/// </summary>
public sealed class DeepSeekChatModelProvider : IChatModelProvider
{
    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetModels() =>
    [
        new("deepseek-chat", "DeepSeek Chat"),
        new("deepseek-coder", "DeepSeek Coder"),
        new("deepseek-reasoner", "DeepSeek Reasoner"),
    ];
}
