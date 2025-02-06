// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Azure.Models;
using Richasy.AgentKernel.Connectors.IFlyTek.Models;
using Richasy.AgentKernel.Models;
using System.Diagnostics.CodeAnalysis;

namespace Consoles.Draw;

internal static class ConfigExtensions
{
    public static AIServiceConfig ToAIServiceConfig(this AzureOpenAIConfiguration? config)
    {
        return config is null || string.IsNullOrWhiteSpace(config.AccessKey) || string.IsNullOrEmpty(config.Endpoint)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : new AzureOpenAIServiceConfig(config.AccessKey, string.Empty, new(config.Endpoint));
    }

    public static AIServiceConfig? ToAIServiceConfig<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this SecretConfiguration? config)
        where T : AIServiceConfig
    {
        return config is null || string.IsNullOrWhiteSpace(config.AccessKey) || string.IsNullOrEmpty(config.Secret)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : Activator.CreateInstance(typeof(T), config.AccessKey, config.Secret, string.Empty) as T;
    }

    public static AIServiceConfig ToAIServiceConfig(this SparkConfiguration? config)
    {
        return config is null || string.IsNullOrWhiteSpace(config.AccessKey) || string.IsNullOrEmpty(config.AppId)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : new SparkDrawServiceConfig(config.AccessKey, config.Secret, config.AppId, string.Empty);
    }
}
