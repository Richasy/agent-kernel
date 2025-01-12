// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Perplexity;

/// <summary>
/// Provides chat models for the Perplexity connector.
/// </summary>
public sealed class PerplexityChatModelProvider : IChatModelProvider
{
    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetModels() =>
    [
        new("llama-3.1-sonar-small-128k-online", "Llama 3.1 Sonar Small 128K Online"),
        new("llama-3.1-sonar-large-128k-online", "Llama 3.1 Sonar Large 128K Online"),
        new("llama-3.1-sonar-huge-128k-online", "Llama 3.1 Sonar Huge 128K Online"),
    ];
}
