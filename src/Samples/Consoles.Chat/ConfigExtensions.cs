// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel;
using Richasy.AgentKernel.Connectors.Baidu.Models;
using Richasy.AgentKernel.Connectors.Ollama.Models;
using Richasy.AgentKernel.Connectors.OpenAI.Models;

namespace Consoles.Chat;

internal static class ConfigExtensions
{
    public static AIServiceConfig ToAIServiceConfig(this OpenAIConfiguration? config)
    {
        var endpoint = string.IsNullOrEmpty(config?.Endpoint) ? null : new Uri(config.Endpoint);
        return config is null || string.IsNullOrWhiteSpace(config.AccessKey)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : new OpenAIServiceConfig(config.AccessKey, config.Model ?? string.Empty, endpoint, config.Organization);
    }

    public static AIServiceConfig? ToAIServiceConfig<TAIServiceConfig>(this EndpointConfiguration? config)
        where TAIServiceConfig : AIServiceConfig
    {
        return config is null || string.IsNullOrWhiteSpace(config.AccessKey)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : Activator.CreateInstance(typeof(TAIServiceConfig), config.AccessKey, config.Model, string.IsNullOrEmpty(config.Endpoint) ? default : new Uri(config.Endpoint)) as TAIServiceConfig;
    }

    public static AIServiceConfig? ToAIServiceConfig<TAIServiceConfig>(this KeyConfiguration? config)
        where TAIServiceConfig : AIServiceConfig
    {
        return config is null || string.IsNullOrWhiteSpace(config.AccessKey)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : Activator.CreateInstance(typeof(TAIServiceConfig), config.AccessKey, config.Model) as TAIServiceConfig;
    }

    public static AIServiceConfig? ToAIServiceConfig(this OllamaConfiguration? config)
    {
        return config is null || string.IsNullOrWhiteSpace(config.Endpoint)
             ? throw new ArgumentException("The configuration is not valid.", nameof(config))
             : new OllamaServiceConfig(config.Model, new Uri(config.Endpoint));
    }

    public static AIServiceConfig? ToAIServiceConfig(this ErnieConfiguration? config)
    {
        return config is null || string.IsNullOrWhiteSpace(config.AccessKey)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : new ErnieServiceConfig(config.AccessKey, config.SecretKey, config.Model ?? string.Empty);
    }
}
