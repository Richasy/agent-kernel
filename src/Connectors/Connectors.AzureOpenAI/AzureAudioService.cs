// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Audio;
using Richasy.AgentKernel.Connectors.Azure.Core;
using Richasy.AgentKernel.Connectors.Azure.Models;
using Richasy.AgentKernel.Models;
using RichasyKernel;

namespace Richasy.AgentKernel.Connectors.Azure;

/// <summary>
/// Represents a service that can generate audio using Azure services.
/// </summary>
public sealed class AzureAudioService : IAudioService
{
    private AzureAudioServiceConfig? _config;

    /// <inheritdoc/>
    public IAudioClient? Client { get; set; }

    /// <inheritdoc/>
    public AIServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig? config)
    {
        if (config is not AzureAudioServiceConfig azureConfig)
        {
            throw new KernelException("Configuration is invalid");
        }

        if (_config != null && azureConfig.Equals(_config))
        {
            return;
        }

        _config = azureConfig;
        Client?.Dispose();
        Client = new AzureAudioClient(azureConfig);
    }
}
