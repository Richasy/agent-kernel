// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Ollama.Models;

/// <summary>
/// Ollama service configuration.
/// </summary>
public sealed class OllamaServiceConfig(string model, Uri? endpoint) : AIServiceConfig(string.Empty, model)
{
    /// <summary>
    /// Gets or sets the endpoint of the service.
    /// </summary>
    public Uri? Endpoint { get; set; } = endpoint;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is OllamaServiceConfig config && EqualityComparer<Uri>.Default.Equals(Endpoint, config.Endpoint) && AccessKey == config.AccessKey;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Endpoint, AccessKey);
}
