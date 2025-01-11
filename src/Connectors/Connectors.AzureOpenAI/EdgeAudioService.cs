// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Audio;
using Richasy.AgentKernel.Connectors.Azure.Core;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Azure;

/// <summary>
/// Azure audio service configuration.
/// </summary>
public sealed class EdgeAudioService : IAudioService
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

        Client = new EdgeAudioClient();
    }
}
