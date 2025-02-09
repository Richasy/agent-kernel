// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Tencent.Models;

/// <summary>
/// Tencent Cloud Translation Service Configuration.
/// </summary>
public sealed class TencentTranslateServiceConfig(string secretId, string secretKey) : TranslateServiceConfig(secretKey)
{
    /// <summary>
    /// Tencent Cloud Translation Service SecretId.
    /// </summary>
    public string SecretId { get; set; } = secretId;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is TencentTranslateServiceConfig config && base.Equals(obj) && AccessKey == config.AccessKey && SecretId == config.SecretId;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), AccessKey, SecretId);
}
