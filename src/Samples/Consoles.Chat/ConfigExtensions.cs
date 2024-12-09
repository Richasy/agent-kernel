// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel;
using Richasy.AgentKernel.Connectors.AzureOpenAI.Models;

namespace Consoles.Chat;

internal static class ConfigExtensions
{
    public static AIServiceConfig ToAIServiceConfig(this AzureOpenAIConfiguration? config)
    {
        return config is null || string.IsNullOrWhiteSpace(config.Endpoint) || string.IsNullOrWhiteSpace(config.AccessKey) || string.IsNullOrWhiteSpace(config.Model)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : new AzureOpenAIServiceConfig(new Uri(config.Endpoint), config.AccessKey, config.Model);
    }
}
