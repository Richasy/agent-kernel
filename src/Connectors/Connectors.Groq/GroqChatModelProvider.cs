// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Groq;

/// <summary>
/// Provides chat models for the Groq connector.
/// </summary>
public sealed class GroqChatModelProvider : IChatModelProvider
{
    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetModels() =>
    [
        new("gemma2-9b-it", "Gemma2 9B"),
        new("llama-3.3-70b-versatile", "Llama 3.3 70B"),
        new("llama-3.1-8b-instant", "Llama 3.1 8B"),
        new("llama3-70b-8192","Llama3 70B 8192"),
        new("mixtral-8x7b-32768","Mixtral 8x7B 32768"),
    ];
}
