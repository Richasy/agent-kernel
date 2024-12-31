// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.OpenRouter.Models;

/// <summary>
/// OpenRouter configuration.
/// </summary>
public sealed class OpenRouterServiceConfig(string key, string model) : AIServiceConfig
{
    /// <summary>
    /// Access Key.
    /// </summary>
    public string AccessKey { get; set; } = key;

    /// <summary>
    /// Model.
    /// </summary>
    public string Model { get; set; } = model;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is OpenRouterServiceConfig config && AccessKey == config.AccessKey && Model == config.Model;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(AccessKey, Model);
}
