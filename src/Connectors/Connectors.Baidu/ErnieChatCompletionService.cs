// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using OpenAI;
using Richasy.AgentKernel.ChatCompletion;
using Richasy.AgentKernel.Connectors.Baidu.Models;

namespace Richasy.AgentKernel.Connectors.Baidu;

/// <summary>
/// 文心一言 Chat Completion Service.
/// </summary>
public sealed class ErnieChatCompletionService : IChatCompletionService
{
    private ErnieServiceConfig? _config;

    /// <inheritdoc/>
    public IChatClient? Client
    {
        get => field ?? throw new InvalidOperationException("The service has not been initialized.");
        set;
    }

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig config)
    {
        if (config is not ErnieServiceConfig qwenConfig)
        {
            throw new ArgumentException("The configuration is not valid.", nameof(config));
        }

        if (_config != null && qwenConfig.Equals(_config))
        {
            return;
        }

        _config = qwenConfig;
        var coreClient = new OpenAIClient(new(_config.AccessKey), new OpenAIClientOptions
        {
            Endpoint = new Uri("https://qianfan.baidubce.com/v2"),
        });

        Client = coreClient.AsChatClient(_config.Model);
    }
}
