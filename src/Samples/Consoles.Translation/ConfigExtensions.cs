// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Azure.Models;
using Richasy.AgentKernel.Models;
using System.Diagnostics.CodeAnalysis;

namespace Consoles.Translation;

internal static class ConfigExtensions
{
    public static TranslateServiceConfig ToTranslationServiceConfig(this AzureConfiguration? config)
    {
        return config is null || string.IsNullOrWhiteSpace(config.AccessKey)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : new AzureTranslationServiceConfig(config.AccessKey, config.Region ?? string.Empty);
    }

    public static TranslateServiceConfig? ToTranslationServiceConfig<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TConfig>(this KeyConfiguration? config)
        where TConfig : TranslateServiceConfig
    {
        return config is null || string.IsNullOrWhiteSpace(config.AccessKey)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : Activator.CreateInstance(typeof(TConfig), config.AccessKey) as TranslateServiceConfig;
    }

    public static TranslateServiceConfig? ToTranslationServiceConfig<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TConfig>(this SecretConfiguration? config)
        where TConfig : TranslateServiceConfig
    {
        return config is null || string.IsNullOrWhiteSpace(config.AccessKey) || string.IsNullOrWhiteSpace(config.Secret)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : Activator.CreateInstance(typeof(TConfig), config.AccessKey, config.Secret) as TranslateServiceConfig;
    }

    public static TranslateServiceConfig? ToTranslationServiceConfig<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TConfig>(this IdConfiguration? config)
        where TConfig : TranslateServiceConfig
    {
        return config is null || string.IsNullOrWhiteSpace(config.AccessKey) || string.IsNullOrWhiteSpace(config.SecretId)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : Activator.CreateInstance(typeof(TConfig), config.SecretId, config.AccessKey) as TranslateServiceConfig;
    }
}
