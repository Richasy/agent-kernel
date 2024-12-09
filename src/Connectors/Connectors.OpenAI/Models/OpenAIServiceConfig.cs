// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.OpenAI.Models;

/// <summary>
/// Configuration for OpenAI service.
/// </summary>
public class OpenAIServiceConfig(string key, string model, Uri? endpoint, string? organization) : AIServiceConfig
{
    /// <summary>
    /// Gets or sets the endpoint of the service.
    /// </summary>
    public Uri? Endpoint { get; set; } = endpoint;

    /// <summary>
    /// Gets or sets the access key of the service.
    /// </summary>
    public string AccessKey { get; set; } = key;

    /// <summary>
    /// Gets or sets the model of the service.
    /// </summary>
    public string Model { get; set; } = model;

    /// <summary>
    /// Gets or sets the organization of the service.
    /// </summary>
    public string? Organization { get; set; } = organization;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is OpenAIServiceConfig config && EqualityComparer<Uri>.Default.Equals(Endpoint, config.Endpoint) && AccessKey == config.AccessKey && Model == config.Model && Organization == config.Organization;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Endpoint, AccessKey, Model, Organization);
}
