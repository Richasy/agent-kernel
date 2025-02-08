// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel;
using Richasy.AgentKernel.Connectors.Azure.Models;
using Richasy.AgentKernel.Connectors.Baidu.Models;
using Richasy.AgentKernel.Connectors.IFlyTek.Models;
using Richasy.AgentKernel.Connectors.OpenAI.Models;
using Richasy.AgentKernel.Connectors.Tencent.Models;
using Richasy.AgentKernel.Models;

namespace Consoles.Draw;

internal static class ConfigExtensions
{
    public static AIServiceConfig ToAIServiceConfig(this AzureOpenAIDrawConfig? config)
    {
        return config is null || string.IsNullOrWhiteSpace(config.Key) || string.IsNullOrEmpty(config.Endpoint)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : new AzureOpenAIServiceConfig(config.Key, string.Empty, new(config.Endpoint));
    }

    public static AIServiceConfig? ToAIServiceConfig(this OpenAIDrawConfig? config)
    {
        var endpoint = string.IsNullOrEmpty(config?.Endpoint) ? null : new Uri(config.Endpoint);
        return config is null || string.IsNullOrWhiteSpace(config.Key)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : new OpenAIServiceConfig(config.Key, string.Empty, endpoint, config.OrganizationId);
    }

    public static AIServiceConfig? ToAIServiceConfig(this ErnieDrawConfig? config)
    {
        return config is null || string.IsNullOrWhiteSpace(config.Key) || string.IsNullOrEmpty(config.Secret)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : new ErnieServiceConfig(config.Key, config.Secret!, string.Empty);
    }

    public static AIServiceConfig? ToAIServiceConfig(this HunyuanDrawConfig? config)
    {
        return config is null || string.IsNullOrWhiteSpace(config.Key) || string.IsNullOrEmpty(config.Secret)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : new HunyuanDrawServiceConfig(config.Key, config.Secret, string.Empty);
    }

    public static AIServiceConfig ToAIServiceConfig(this SparkDrawConfig? config)
    {
        return config is null || string.IsNullOrWhiteSpace(config.Key) || string.IsNullOrEmpty(config.AppId)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : new SparkDrawServiceConfig(config.Key, config.Secret!, config.AppId, string.Empty);
    }
}
