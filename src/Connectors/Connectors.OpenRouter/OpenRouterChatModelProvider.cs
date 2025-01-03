// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.ChatCompletion;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.OpenRouter;

/// <summary>
/// Provides chat models for the OpenRouter connector.
/// </summary>
public sealed class OpenRouterChatModelProvider : IChatModelProvider
{
    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetModels() =>
    [
        new("deepseek/deepseek-chat", "DeepSeek V3"),
        new("qwen/qvq-72b-preview", "Qwen: QVQ 72B Preview"),
        new("google/gemini-2.0-flash-thinking-exp:free", "Google: Gemini 2.0 Flash Thinking Experimental"),
        new("sao10k/l3.3-euryale-70b", "Sao10K: Llama 3.3 Euryale 70B"),
        new("openai/o1", "OpenAI: o1"),
    ];
}
