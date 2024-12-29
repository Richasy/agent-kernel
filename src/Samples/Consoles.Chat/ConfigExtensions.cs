// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Connectors.DeepSeek.Models;
using Richasy.AgentKernel;
using Richasy.AgentKernel.Connectors.Ali.Models;
using Richasy.AgentKernel.Connectors.Anthropic.Models;
using Richasy.AgentKernel.Connectors.AzureOpenAI.Models;
using Richasy.AgentKernel.Connectors.Baidu.Models;
using Richasy.AgentKernel.Connectors.Gemini.Models;
using Richasy.AgentKernel.Connectors.LingYi.Models;
using Richasy.AgentKernel.Connectors.Moonshot.Models;
using Richasy.AgentKernel.Connectors.OpenAI.Models;
using Richasy.AgentKernel.Connectors.XAI.Models;
using Richasy.AgentKernel.Connectors.ZhiPu.Models;

namespace Consoles.Chat;

internal static class ConfigExtensions
{
    public static AIServiceConfig ToAIServiceConfig(this OpenAIConfiguration? config)
    {
        var endpoint = string.IsNullOrEmpty(config?.Endpoint) ? null : new Uri(config.Endpoint);
        return config is null || string.IsNullOrWhiteSpace(config.AccessKey) || string.IsNullOrWhiteSpace(config.Model)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : new OpenAIServiceConfig(config.AccessKey, config.Model, endpoint, config.Organization);
    }

    public static AIServiceConfig ToAIServiceConfig(this AzureOpenAIConfiguration? config)
    {
        return config is null || string.IsNullOrWhiteSpace(config.Endpoint) || string.IsNullOrWhiteSpace(config.AccessKey) || string.IsNullOrWhiteSpace(config.Model)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : new AzureOpenAIServiceConfig(new Uri(config.Endpoint), config.AccessKey, config.Model);
    }

    public static AIServiceConfig ToAIServiceConfig(this XAIConfiguration? config)
    {
        return config is null || string.IsNullOrWhiteSpace(config.AccessKey) || string.IsNullOrWhiteSpace(config.Model)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : new XAIServiceConfig(config.AccessKey, config.Model);
    }

    public static AIServiceConfig ToAIServiceConfig(this ZhiPuConfiguration? config)
    {
        return config is null || string.IsNullOrWhiteSpace(config.AccessKey) || string.IsNullOrWhiteSpace(config.Model)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : new ZhiPuServiceConfig(config.AccessKey, config.Model);
    }

    public static AIServiceConfig ToAIServiceConfig(this LingYiConfiguration? config)
    {
        return config is null || string.IsNullOrWhiteSpace(config.AccessKey) || string.IsNullOrWhiteSpace(config.Model)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : new LingYiServiceConfig(config.AccessKey, config.Model);
    }

    public static AIServiceConfig ToAIServiceConfig(this AnthropicConfiguration? config)
    {
        return config is null || string.IsNullOrWhiteSpace(config.AccessKey)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : new AnthropicServiceConfig(config.AccessKey, config.Model, string.IsNullOrEmpty(config.Endpoint) ? default : new(config.Endpoint));
    }

    public static AIServiceConfig ToAIServiceConfig(this MoonshotConfiguration? config)
    {
        return config is null || string.IsNullOrEmpty(config.AccessKey) || string.IsNullOrEmpty(config.Model)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : new MoonshotServiceConfig(config.AccessKey, config.Model);
    }

    public static AIServiceConfig ToAIServiceConfig(this GeminiConfiguration? config)
    {
        var endpoint = string.IsNullOrEmpty(config?.Endpoint) ? null : new Uri(config.Endpoint);
        return config is null || string.IsNullOrEmpty(config.AccessKey) || string.IsNullOrEmpty(config.Model)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : new GeminiServiceConfig(config.AccessKey, config.Model, endpoint);
    }

    public static AIServiceConfig ToAIServiceConfig(this DeepSeekConfiguration? config)
    {
        return config is null || string.IsNullOrEmpty(config.AccessKey) || string.IsNullOrEmpty(config.Model)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : new DeepSeekServiceConfig(config.AccessKey, config.Model);
    }

    public static AIServiceConfig ToAIServiceConfig(this QwenConfiguration? config)
    {
        return config is null || string.IsNullOrEmpty(config.AccessKey) || string.IsNullOrEmpty(config.Model)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : new QwenServiceConfig(config.AccessKey, config.Model);
    }
    
    public static AIServiceConfig ToAIServiceConfig(this ErnieConfiguration? config)
    {
        return config is null || string.IsNullOrEmpty(config.AccessKey) || string.IsNullOrEmpty(config.Model)
            ? throw new ArgumentException("The configuration is not valid.", nameof(config))
            : new ErnieServiceConfig(config.AccessKey, config.Model);
    }
}
