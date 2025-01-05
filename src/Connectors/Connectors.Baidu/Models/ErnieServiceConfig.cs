// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Baidu.Models;

/// <summary>
/// 文心一言服务配置.
/// </summary>
public sealed class ErnieServiceConfig(string key, string secret, string model) : AIServiceConfig(key, model)
{
    /// <summary>
    /// 获取或设置SecretKey.
    /// </summary>
    public string SecretKey { get; set; } = secret;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is ErnieServiceConfig config && base.Equals(obj) && AccessKey == config.AccessKey && SecretKey == config.SecretKey;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), AccessKey, SecretKey);
}
