// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Azure.Models;

/// <summary>
/// Configuration for Azure OpenAI service.
/// </summary>
public class AzureOpenAIServiceConfig(string key, string model, Uri endpoint) : AIServiceConfig(key, model)
{
    /// <summary>
    /// Gets or sets the endpoint of the service.
    /// </summary>
    public Uri Endpoint { get; set; } = endpoint;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is AzureOpenAIServiceConfig config && EqualityComparer<Uri>.Default.Equals(Endpoint, config.Endpoint) && AccessKey == config.AccessKey;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Endpoint, AccessKey);
}
