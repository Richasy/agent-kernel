// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.ChatCompletion;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Moonshot;

/// <summary>
/// Provides chat models for the Moonshot connector.
/// </summary>
public sealed class MoonshotChatModelProvider : IChatModelProvider
{
    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetModels() =>
    [
        new("moonshot-v1-8k", "Moonshot V1 8K"),
        new("moonshot-v1-128k", "Moonshot V1 128K"),
        new("moonshot-v1-32k", "Moonshot V1 32K"),
        new("moonshot-v1-auto", "Moonshot V1 Auto"),
    ];
}
