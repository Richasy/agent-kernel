// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Volcano.Models;

/// <summary>
/// 火山AI音频服务配置.
/// </summary>
public sealed class VolcanoAudioServiceConfig(string accessKey, string appId, string? model) : AIServiceConfig(accessKey, model)
{
    /// <summary>
    /// 应用 ID.
    /// </summary>
    public string AppId { get; set; } = appId;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is VolcanoAudioServiceConfig config && base.Equals(obj) && AccessKey == config.AccessKey && AppId == config.AppId;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), AccessKey, AppId);
}
