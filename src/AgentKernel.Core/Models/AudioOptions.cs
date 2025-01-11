// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Models;

/// <summary>
/// Represents the options for an audio request.
/// </summary>
public class AudioOptions
{
    /// <summary>
    /// Gets or sets the model identifier.
    /// </summary>
    public string? ModelId { get; set; }

    /// <summary>
    /// Gets or sets the voice identifier.
    /// </summary>
    public string? VoiceId { get; set; }

    /// <summary>
    /// Gets or sets the speed of the audio.
    /// </summary>
    public double? Speed { get; set; }

    /// <summary>
    /// Gets or sets the language of the voice.
    /// </summary>
    public string? LanguageCode { get; set; }
}
