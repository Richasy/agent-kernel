// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel;
using Richasy.AgentKernel.Connectors.Azure.Models;
using Richasy.AgentKernel.Connectors.OpenAI.Models;
using Richasy.AgentKernel.Connectors.Tencent.Models;
using Richasy.AgentKernel.Connectors.Volcano.Models;
using Richasy.AgentKernel.Models;

namespace Consoles.Audio;

internal static class ConfigExtensions
{
    public static AIServiceConfig ToAIServiceConfig(this OpenAIAudioConfig? config)
    {
        var endpoint = string.IsNullOrEmpty(config?.Endpoint) ? null : new Uri(config.Endpoint);
        return config is null || string.IsNullOrWhiteSpace(config.Key)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : new OpenAIServiceConfig(config.Key, string.Empty, endpoint, config.OrganizationId);
    }

    public static AIServiceConfig ToAIServiceConfig(this AzureOpenAIAudioConfig? config)
    {
        return config is null || string.IsNullOrWhiteSpace(config.Key) || string.IsNullOrEmpty(config.Endpoint)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : new AzureOpenAIServiceConfig(config.Key, string.Empty, new(config.Endpoint));
    }

    public static AIServiceConfig ToAIServiceConfig(this AzureAudioConfig? config)
    {
        return config is null || string.IsNullOrWhiteSpace(config.Key) || string.IsNullOrEmpty(config.Region)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : new AzureAudioServiceConfig(config.Key, config.Region);
    }

    public static AIServiceConfig ToAIServiceConfig(this VolcanoAudioConfig? config)
    {
        return config is null || string.IsNullOrWhiteSpace(config.Key) || string.IsNullOrEmpty(config.AppId)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : new VolcanoAudioServiceConfig(config.Key, config.AppId, string.Empty);
    }

    public static AIServiceConfig ToAIServiceConfig(this TencentAudioConfig? config)
    {
        return config is null || string.IsNullOrWhiteSpace(config.Key) || string.IsNullOrEmpty(config.SecretId)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : new TencentAudioServiceConfig(config.SecretId, config.Key, string.Empty);
    }
}
