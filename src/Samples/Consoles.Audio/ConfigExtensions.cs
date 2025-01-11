// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Azure.Models;
using Richasy.AgentKernel.Models;

namespace Consoles.Audio;

internal static class ConfigExtensions
{
    public static AIServiceConfig ToAIServiceConfig(this AzureConfiguration? config)
    {
        return config is null || string.IsNullOrWhiteSpace(config.AccessKey) || string.IsNullOrEmpty(config.Region)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : new AzureAudioServiceConfig(config.AccessKey, config.Region);
    }
}
