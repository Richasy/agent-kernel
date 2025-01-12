// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Azure.Models;
using Richasy.AgentKernel.Models;

namespace Consoles.Draw;

internal static class ConfigExtensions
{
    public static AIServiceConfig ToAIServiceConfig(this AzureOpenAIConfiguration? config)
    {
        return config is null || string.IsNullOrWhiteSpace(config.AccessKey) || string.IsNullOrEmpty(config.Endpoint) || string.IsNullOrEmpty(config.Model)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : new AzureOpenAIServiceConfig(config.AccessKey, config.Model, new(config.Endpoint));
    }
}
