// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel;

/// <summary>
/// 绘图配置管理器.
/// </summary>
public interface IDrawConfigManager
{
    /// <summary>
    /// 获取绘图客户端配置.
    /// </summary>
    DrawClientConfiguration? Configuration { get; }

    /// <summary>
    /// 获取绘图客户端配置.
    /// </summary>
    /// <param name="provider">供应商.</param>
    /// <returns>配置.</returns>
    Task<DrawClientConfigBase?> GetDrawConfigAsync(DrawProviderType provider);

    /// <summary>
    /// 获取绘图服务配置.
    /// </summary>
    Task<AIServiceConfig?> GetServiceConfigAsync(DrawProviderType provider, DrawModel model);

    /// <summary>
    /// 保存绘图客户端配置.
    /// </summary>
    /// <param name="configMap">配置列表.</param>
    /// <returns><see cref="Task"/>.</returns>
    Task SaveDrawConfigAsync(Dictionary<DrawProviderType, DrawClientConfigBase> configMap);
}
