// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Azure.Models.Audio;

/// <summary>
/// Options for generating audio using Azure services.
/// </summary>
public sealed class AzureAudioOptions : AudioOptions
{
    /// <summary>
    /// The gender of the voice.
    /// </summary>
    public string? Gender { get; set; }
}
