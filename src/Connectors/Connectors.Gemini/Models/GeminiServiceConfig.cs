// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Gemini.Models;

/// <summary>
/// Gemini Service Configuration.
/// </summary>
public sealed class GeminiServiceConfig(string key, string model, Uri? endpoint) : AIServiceConfig
{
    /// <summary>
    /// Gets or sets the endpoint of the service.
    /// </summary>
    public Uri? Endpoint { get; set; } = endpoint;

    /// <summary>
    /// Access Key.
    /// </summary>
    public string AccessKey { get; set; } = key;

    /// <summary>
    /// Model.
    /// </summary>
    public string Model { get; set; } = model;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is GeminiServiceConfig config && EqualityComparer<Uri>.Default.Equals(Endpoint, config.Endpoint) && AccessKey == config.AccessKey && Model == config.Model;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(AccessKey, Model);
}
