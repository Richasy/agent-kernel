// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Youdao.Models;

/// <summary>
/// Youdao translation service configuration.
/// </summary>
public sealed class YoudaoTranslationServiceConfig(string appId, string secret) : TranslateServiceConfig(secret)
{
    /// <summary>
    /// The app id of the translation service.
    /// </summary>
    public string AppId { get; set; } = appId;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is YoudaoTranslationServiceConfig config && base.Equals(obj) && AccessKey == config.AccessKey && AppId == config.AppId;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), AccessKey, AppId);
}
