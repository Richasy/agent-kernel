// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Models;

/// <summary>
/// Represents the configuration of an AI service.
/// </summary>
public abstract class AIServiceConfig(string key, string? model)
{
    /// <summary>
    /// Access Key.
    /// </summary>
    public string AccessKey { get; set; } = key;

    /// <summary>
    /// Model.
    /// </summary>
    public string? Model { get; set; } = model;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is AIServiceConfig config && AccessKey == config.AccessKey;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(AccessKey);
}
