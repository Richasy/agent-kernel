// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Audio;

/// <summary>
/// Represents a client that can generate audio.
/// </summary>
public interface IAudioClient : IDisposable
{
    /// <summary>
    /// Gets metadata that describes the <see cref="IAudioClient"/>.
    /// </summary>
    public AudioClientMetadata Metadata { get; }

    /// <summary>
    /// Converts text to speech.
    /// </summary>
    /// <returns>Audio data.</returns>
    Task<BinaryData> TextToSpeechAsync(
        string text,
        AudioOptions? options,
        CancellationToken cancellationToken = default);
}
