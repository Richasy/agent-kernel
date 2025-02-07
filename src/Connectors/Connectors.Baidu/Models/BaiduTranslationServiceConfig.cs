// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Baidu.Models;

/// <summary>
/// Baidu Translation Service Configuration.
/// </summary>
public sealed class BaiduTranslationServiceConfig(string appId, string secret) : TranslateServiceConfig(appId)
{
    /// <summary>
    /// Translation Service Secret
    /// </summary>
    public string Secret { get; set; } = secret!;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is BaiduTranslationServiceConfig config && base.Equals(obj) && AccessKey == config.AccessKey && Secret == config.Secret;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), AccessKey, Secret);
}
