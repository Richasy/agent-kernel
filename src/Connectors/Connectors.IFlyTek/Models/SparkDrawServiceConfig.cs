// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.IFlyTek.Models;

/// <summary>
/// Configuration for the Spark draw service.
/// </summary>
public sealed class SparkDrawServiceConfig(string key, string secret, string appId, string? model) : AIServiceConfig(key, model)
{
    /// <summary>
    /// The secret key for the service.
    /// </summary>
    public string Secret { get; set; } = secret;

    /// <summary>
    /// The application appid, obtained from the open platform control panel.
    /// </summary>
    public string AppId { get; set; } = appId;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is SparkDrawServiceConfig config && base.Equals(obj) && AccessKey == config.AccessKey && Secret == config.Secret && AppId == config.AppId;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), AccessKey, Secret, AppId);
}
