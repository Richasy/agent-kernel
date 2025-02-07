// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.


namespace Richasy.AgentKernel.Models;

/// <summary>
/// Configuration for a translate service.
/// </summary>
/// <param name="accessKey"></param>
public abstract class TranslateServiceConfig(string accessKey)
{
    /// <summary>
    /// The access key for the translation service.
    /// </summary>
    public string AccessKey { get; set; } = accessKey;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is TranslateServiceConfig config && AccessKey == config.AccessKey;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(AccessKey);
}
