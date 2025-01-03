// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.ChatCompletion;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Ali;

/// <summary>
/// 千问聊天模型提供程序.
/// </summary>
public sealed class QwenChatModelProvider : IChatModelProvider
{
    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetModels()
    {
        return
        [
            new("qwen-max", "通义千问-Max", toolSupport: true),
            new("qwen-plus", "通义千问-Plus", toolSupport: true),
            new("qwen-turbo", "通义千问-Turbo", toolSupport: true),
            new("qwen-long", "Qwen-Long"),
            new("qwen-vl-max", "通义千问 VL", visionSupport: true),
            new("qwen2.5-72b-instruct", "通义千问 2.5-开源版"),
            new("qwen2-72b", "通义千问 2-开源版"),
            new("qwen1.5-110b-chat", "通义千问 1.5-开源版"),
            new("qwen-72b-chat", "通义千问 1-开源版"),
        ];
    }
}
