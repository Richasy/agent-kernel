// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Audio;

namespace Richasy.AgentKernel.Models;

/// <summary>
/// Provides metadata about an <see cref="IAudioClient"/>.
/// </summary>
public class AudioClientMetadata(string? providerName, string? modelName)
{
    /// <summary>
    /// Gets the name of the audio provider.
    /// </summary>
    public string? ProviderName { get; } = providerName;

    /// <summary>
    /// Gets the name of the audio model.
    /// </summary>
    public string? ModelName { get; } = modelName;
}
