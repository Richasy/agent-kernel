// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.ChatCompletion;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Gemini;

/// <summary>
/// Gemini chat model provider.
/// </summary>
public sealed class GeminiChatModelProvider : IChatModelProvider
{
    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetModels() =>
    [
        new("gemini-2.0-flash-exp", "Gemini 2.0 Flash", toolSupport: true, visionSupport: true),
        new("gemini-1.5-flash", "Gemini 1.5 Flash", toolSupport: true, visionSupport: true),
        new("gemini-1.5-flash-8b", "Gemini 1.5 Flash-8B", toolSupport: true, visionSupport: true),
        new("gemini-1.5-pro", "Gemini 1.5 Pro", toolSupport: true, visionSupport: true),
    ];
}
