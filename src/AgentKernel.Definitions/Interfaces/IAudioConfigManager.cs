// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel;

/// <summary>
/// 音频配置管理器.
/// </summary>
public interface IAudioConfigManager
{
    /// <summary>
    /// 获取音频客户端配置.
    /// </summary>
    AudioClientConfiguration? Configuration { get; }

    /// <summary>
    /// 获取音频客户端配置.
    /// </summary>
    /// <param name="provider">供应商.</param>
    /// <returns>配置.</returns>
    Task<AudioClientConfigBase?> GetAudioConfigAsync(AudioProviderType provider);

    /// <summary>
    /// 获取语音服务配置.
    /// </summary>
    Task<AIServiceConfig?> GetServiceConfigAsync(AudioProviderType provider, AudioModel model);

    /// <summary>
    /// 保存音频客户端配置.
    /// </summary>
    /// <param name="configMap">配置列表.</param>
    /// <returns><see cref="Task"/>.</returns>
    Task SaveAudioConfigAsync(Dictionary<AudioProviderType, AudioClientConfigBase> configMap);
}
