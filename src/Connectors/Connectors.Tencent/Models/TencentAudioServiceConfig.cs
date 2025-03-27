// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Tencent.Models;

/// <summary>
/// Tencent Cloud TTS Service Configuration.
/// </summary>
public sealed class TencentAudioServiceConfig(string secretKey, string secretId,  string? model) : AIServiceConfig(secretKey, model)
{
    /// <summary>
    /// Tencent Cloud TTS Service SecretId.
    /// </summary>
    public string SecretId { get; set; } = secretId;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is TencentAudioServiceConfig config && base.Equals(obj) && AccessKey == config.AccessKey && SecretId == config.SecretId;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), AccessKey, SecretId);
}
