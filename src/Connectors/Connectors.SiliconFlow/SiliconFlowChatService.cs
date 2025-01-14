// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using AgentKernel.Core.OpenAI;
using AgentKernel.Extensions.AI;
using Microsoft.Extensions.AI;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.SiliconFlow.Models;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.SiliconFlow;

/// <summary>
/// 硅基流动 Chat Completion Service.
/// </summary>
public sealed class SiliconFlowChatService : IChatService
{
    private SiliconFlowServiceConfig? _config;

    /// <inheritdoc/>
    public IChatClient? Client { get; set; }

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

        Client?.Dispose();
        Client = coreClient.AsChatClient(_config.Model!);
    }
}
