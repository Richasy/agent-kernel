// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Audio;

/// <summary>
/// Represents a service that can generate audio.
/// </summary>
public interface IAudioService
{
    /// <summary>
    /// Gets the client that is used to generate audio.
    /// </summary>
    IAudioClient? Client { get; }

    /// <summary>
    /// Gets the configuration of the audio service.
    /// </summary>
    AIServiceConfig? Config { get; }

    /// <summary>
    /// Initialize the audio service.
    /// </summary>
    void Initialize(AIServiceConfig? config);
}
