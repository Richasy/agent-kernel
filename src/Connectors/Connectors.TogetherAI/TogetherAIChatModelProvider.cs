// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.ChatCompletion;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.TogetherAI;

/// <summary>
/// Provides chat models for the TogetherAI connector.
/// </summary>
public sealed class TogetherAIChatModelProvider : IChatModelProvider
{
    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetModels() =>
    [
        new("meta-llama/Llama-3.3-70B-Instruct-Turbo", "Llama 3.3 70B Instruct Turbo"),
        new("Qwen/QwQ-32B-Preview", "Qwen QwQ 32B Preview"),
    ];
}
