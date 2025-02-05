// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Audio;
using Richasy.AgentKernel.Connectors.Windows.Core;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Windows;

/// <summary>
/// Windows audio service.
/// </summary>
public sealed class WindowsAudioService : IAudioService
{
    /// <inheritdoc/>
    public IAudioClient? Client { get; set; }

    /// <inheritdoc/>
    public AIServiceConfig? Config => default;

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig? config)
    {
        if (Client is not null)
        {
            return;
        }

        Client = new WindowsAudioClient();
    }
}
