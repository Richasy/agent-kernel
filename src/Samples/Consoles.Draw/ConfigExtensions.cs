// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Azure.Models;
using Richasy.AgentKernel.Models;
using System.Diagnostics.CodeAnalysis;

namespace Consoles.Draw;

internal static class ConfigExtensions
{
    public static AIServiceConfig ToAIServiceConfig(this AzureOpenAIConfiguration? config)
    {
        return config is null || string.IsNullOrWhiteSpace(config.AccessKey) || string.IsNullOrEmpty(config.Endpoint) || string.IsNullOrEmpty(config.Model)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : new AzureOpenAIServiceConfig(config.AccessKey, config.Model, new(config.Endpoint));
    }

    public static AIServiceConfig? ToAIServiceConfig<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this SecretConfiguration? config)
        where T : AIServiceConfig
    {
        return config is null || string.IsNullOrWhiteSpace(config.AccessKey) || string.IsNullOrEmpty(config.Secret) || string.IsNullOrEmpty(config.Model) || string.IsNullOrEmpty(config.Size)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : Activator.CreateInstance(typeof(T), config.AccessKey, config.Secret, config.Model) as T;
    }
}
