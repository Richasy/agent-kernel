// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using OpenAI;
using Richasy.AgentKernel.ChatCompletion;
using Richasy.AgentKernel.Connectors.SiliconFlow.Models;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.SiliconFlow;

/// <summary>
/// 硅基流动 Chat Completion Service.
/// </summary>
public sealed class SiliconFlowChatCompletionService : IChatCompletionService
{
    private SiliconFlowServiceConfig? _config;

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
        if (config is not SiliconFlowServiceConfig sfConfig)
        {
            throw new ArgumentException("The configuration is not valid.", nameof(config));
        }

        if (_config != null && sfConfig.Equals(_config))
        {
            return;
        }

        _config = sfConfig;
        var coreClient = new OpenAIClient(new(_config.AccessKey), new OpenAIClientOptions
        {
            Endpoint = new Uri("https://api.siliconflow.cn/v1"),
        });

        Client = coreClient.AsChatClient(_config.Model!);
    }
}
