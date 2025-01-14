// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Core.OpenAI;
using Richasy.AgentKernel.Extensions.AI;
using Microsoft.Extensions.AI;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.Tencent.Models;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Tencent;

/// <summary>
/// 腾讯混元 Chat Completion Service.
/// </summary>
public sealed class HunyuanChatService : IChatService
{
    private HunyuanChatServiceConfig? _config;

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
        if (config is not HunyuanChatServiceConfig hunyuanConfig)
        {
            throw new ArgumentException("The configuration is not valid.", nameof(config));
        }

        if (_config != null && hunyuanConfig.Equals(_config))
        {
            return;
        }

        _config = hunyuanConfig;
        var coreClient = new OpenAIClient(new(_config.AccessKey), new OpenAIClientOptions
        {
            Endpoint = new Uri("https://api.hunyuan.cloud.tencent.com/v1"),
        });

        Client = coreClient.AsChatClient(_config.Model!);
    }
}
