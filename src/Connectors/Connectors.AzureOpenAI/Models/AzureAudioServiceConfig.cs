// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Azure.Models;

/// <summary>
/// Configuration for an Azure audio service.
/// </summary>
public sealed class AzureAudioServiceConfig(string key, string region) : AIServiceConfig(key, default)
{
    /// <summary>
    /// The region of the Azure service.
    /// </summary>
    public string Region { get; } = region;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is AzureAudioServiceConfig config && base.Equals(obj) && AccessKey == config.AccessKey && Region == config.Region;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), AccessKey, Region);
}
