// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Draw;

namespace Richasy.AgentKernel.Models;

/// <summary>
/// Providers metadata for a <see cref="IDrawClient"/>.
/// </summary>
public class DrawClientMetadata(string? providerName, string? modelId)
{
    /// <summary>
    /// Gets the name of the translation provider.
    /// </summary>
    public string? ProviderName { get; } = providerName;

    /// <summary>
    /// Gets the model identifier.
    /// </summary>
    public string? ModelId { get; } = modelId;
}
