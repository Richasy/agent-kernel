// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.AzureOpenAI.Models;

/// <summary>
/// Configuration for Azure OpenAI service.
/// </summary>
public class AzureOpenAIServiceConfig(string key, string model, Uri endpoint) : AIServiceConfig
{
    /// <summary>
    /// Gets or sets the endpoint of the service.
    /// </summary>
    public Uri Endpoint { get; set; } = endpoint;

    /// <summary>
    /// Gets or sets the access key of the service.
    /// </summary>
    public string AccessKey { get; set; } = key;

    /// <summary>
    /// Gets or sets the model of the service.
    /// </summary>
    public string Model { get; set; } = model;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is AzureOpenAIServiceConfig config && EqualityComparer<Uri>.Default.Equals(Endpoint, config.Endpoint) && AccessKey == config.AccessKey && Model == config.Model;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Endpoint, AccessKey, Model);
}
