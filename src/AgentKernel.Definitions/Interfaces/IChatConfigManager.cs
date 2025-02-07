// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel;

/// <summary>
/// Chat configuration manager.
/// </summary>
public interface IChatConfigManager
{
    /// <summary>
    /// 配置.
    /// </summary>
    ChatClientConfiguration? Configuration { get; }

    /// <summary>
    /// 获取聊天客户端配置.
    /// </summary>
    /// <param name="provider">供应商.</param>
    /// <returns>配置.</returns>
    Task<ChatClientConfigBase?> GetChatConfigAsync(ChatProviderType provider);

    /// <summary>
    /// 获取聊天服务配置.
    /// </summary>
    Task<AIServiceConfig?> GetServiceConfigAsync(ChatProviderType provider, ChatModel model);

    /// <summary>
    /// 保存聊天客户端配置.
    /// </summary>
    /// <param name="configMap">配置列表.</param>
    /// <returns><see cref="Task"/>.</returns>
    Task SaveChatConfigAsync(Dictionary<ChatProviderType, ChatClientConfigBase> configMap);
}
