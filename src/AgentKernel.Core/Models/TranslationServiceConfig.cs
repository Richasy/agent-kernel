// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.


namespace Richasy.AgentKernel.Models;

/// <summary>
/// Configuration for a translation service.
/// </summary>
/// <param name="accessKey"></param>
public abstract class TranslationServiceConfig(string accessKey)
{
    /// <summary>
    /// The access key for the translation service.
    /// </summary>
    public string AccessKey { get; set; } = accessKey;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is TranslationServiceConfig config && AccessKey == config.AccessKey;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(AccessKey);
}
