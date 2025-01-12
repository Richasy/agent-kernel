// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.SiliconFlow;

/// <summary>
/// SiliconFlow chat model provider.
/// </summary>
public sealed class SiliconFlowChatModelProvider : IChatModelProvider
{
    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetModels() =>
    [
        new("Qwen/QVQ-72B-Preview", "Qwen/QVQ-72B-Preview", toolSupport: true, visionSupport: true),
        new("deepseek-ai/DeepSeek-V2.5", "DeepSeek-V2.5"),
    ];
}
