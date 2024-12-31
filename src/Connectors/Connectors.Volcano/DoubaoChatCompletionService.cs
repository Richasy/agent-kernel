// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using OpenAI;
using Richasy.AgentKernel.ChatCompletion;
using Richasy.AgentKernel.Connectors.Volcano.Models;

namespace Richasy.AgentKernel.Connectors.Volcano;

/// <summary>
/// 字节豆包 Chat Completion Service.
/// </summary>
public sealed class DoubaoChatCompletionService : IChatCompletionService
{
    private DoubaoServiceConfig? _config;

    /// <inheritdoc/>
    public IChatClient? Client
    {
        get => field ?? throw new InvalidOperationException("The service has not been initialized.");
        set;
    }

    /// <inheritdoc/>
    public AIServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig config)
    {
        if (config is not DoubaoServiceConfig doubaoConfig)
        {
            throw new ArgumentException("The configuration is not valid.", nameof(config));
        }

        if (_config != null && doubaoConfig.Equals(_config))
        {
            return;
        }

        _config = doubaoConfig;
        var coreClient = new OpenAIClient(new(_config.AccessKey), new OpenAIClientOptions
        {
            Endpoint = new Uri("https://ark.cn-beijing.volces.com/api/v3"),
        });

        Client = coreClient.AsChatClient(_config.Model!);
    }
}