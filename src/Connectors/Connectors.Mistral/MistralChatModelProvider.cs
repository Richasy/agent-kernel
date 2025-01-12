// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Mistral;

/// <summary>
/// Provides chat models for the Mistral connector.
/// </summary>
public sealed class MistralChatModelProvider : IChatModelProvider
{
    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetModels() =>
    [
        new("mistral-large-latest", "Mistral Large", toolSupport: true),
        new("pixtral-large-latest", "Pixtral Large", toolSupport: true, visionSupport: true),
        new("ministral-3b-latest", "Ministral 3B"),
        new("ministral-8b-latest", "Ministral 8B"),
        new("ministral-small-latest", "Ministral Small"),
        new("codestral-latest", "Codestral"),
    ];
}
