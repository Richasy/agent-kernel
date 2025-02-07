// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Ali.Models;

/// <summary>
/// Configuration of the Ali translation service.
/// </summary>
public sealed class AliTranslationServiceConfig(string key, string secret) : TranslateServiceConfig(key)
{
    /// <summary>
    /// The secret of the translation service.
    /// </summary>
    public string Secret { get; set; } = secret;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is AliTranslationServiceConfig config && base.Equals(obj) && AccessKey == config.AccessKey && Secret == config.Secret;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), AccessKey, Secret);
}
