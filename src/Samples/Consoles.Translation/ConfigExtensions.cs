// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Azure.Models;
using Richasy.AgentKernel.Models;

namespace Consoles.Translation;

internal static class ConfigExtensions
{
    public static TranslationServiceConfig ToTranslationServiceConfig(this AzureConfiguration? config)
    {
        return config is null || string.IsNullOrWhiteSpace(config.AccessKey)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : new AzureTranslationServiceConfig(config.AccessKey, config.Region ?? string.Empty);
    }
}
