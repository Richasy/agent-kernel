// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Anthropic.Models;

/// <summary>
/// Anthropic Service Configuration.
/// </summary>
public sealed class AnthropicServiceConfig(string key, string? model, Uri? endpoint) : AIServiceConfig(key, model)
{
    /// <summary>
    /// Gets or sets the endpoint of the service.
    /// </summary>
    public Uri? Endpoint { get; set; } = endpoint;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is AnthropicServiceConfig config && EqualityComparer<Uri?>.Default.Equals(Endpoint, config.Endpoint) && AccessKey == config.AccessKey && Model == config.Model;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Endpoint, AccessKey, Model);
}
