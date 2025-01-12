// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Chat;

/// <summary>
/// 聊天模型提供程序.
/// </summary>
public interface IChatModelProvider
{
    /// <summary>
    /// 获取所有模型.
    /// </summary>
    /// <returns>模型列表.</returns>
    public IReadOnlyList<ChatModel> GetModels();
}
