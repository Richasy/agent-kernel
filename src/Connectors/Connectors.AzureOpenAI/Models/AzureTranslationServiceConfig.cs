// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Azure.Models;

/// <summary>
/// Azure translation service configuration.
/// </summary>
public sealed class AzureTranslationServiceConfig(string key, string? region) : TranslationServiceConfig(key)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AzureTranslationServiceConfig"/> class.
    /// </summary>
    public string? Region { get; set; } = region;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is AzureTranslationServiceConfig config && base.Equals(obj) && AccessKey == config.AccessKey && Region == config.Region;
    
    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), AccessKey, Region);
}
