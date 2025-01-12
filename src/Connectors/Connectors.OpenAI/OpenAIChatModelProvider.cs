// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.OpenAI;

/// <summary>
/// Provides chat models for the OpenAI connector.
/// </summary>
public sealed class OpenAIChatModelProvider : IChatModelProvider
{
    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetModels() =>
    [
        new("gpt-4o", "GPT-4o", toolSupport: true, visionSupport: true),
        new("gpt-4o-mini", "GPT-4o Mini", toolSupport: true, visionSupport: true),
        new("o1", "O1", toolSupport: true, visionSupport: true),
        new("o1-mini", "O1 Mini", toolSupport: true, visionSupport: true),
        new("o1-preview", "O1 Preview", toolSupport: true, visionSupport: true),
        new("gpt-4-turbo", "GPT-4 Turbo", toolSupport: true),
        new("gpt-4", "GPT-4", toolSupport: true),
        new("gpt-3.5-turbo", "GPT-3.5 Turbo"),
    ];
}
