// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel;

/// <summary>
/// 翻译配置管理器.
/// </summary>
public interface ITranslateConfigManager
{
    /// <summary>
    /// 获取翻译客户端配置.
    /// </summary>
    TranslateClientConfiguration? Configuration { get; }

    /// <summary>
    /// 获取翻译客户端配置.
    /// </summary>
    /// <param name="provider">供应商.</param>
    /// <returns>配置.</returns>
    Task<TranslateClientConfigBase?> GetTranslateConfigAsync(TranslateProviderType provider);

    /// <summary>
    /// 获取翻译服务配置.
    /// </summary>
    Task<TranslateServiceConfig?> GetServiceConfigAsync(TranslateProviderType provider);

    /// <summary>
    /// 保存翻译客户端配置.
    /// </summary>
    /// <param name="configMap">配置列表.</param>
    /// <returns><see cref="Task"/>.</returns>
    Task SaveTranslateConfigAsync(Dictionary<TranslateProviderType, TranslateClientConfigBase> configMap);
}
