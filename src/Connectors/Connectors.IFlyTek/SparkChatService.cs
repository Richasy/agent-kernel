// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Core.OpenAI;
using Richasy.AgentKernel.Extensions.AI;
using Microsoft.Extensions.AI;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.IFlyTek.Models;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.IFlyTek;

/// <summary>
/// 讯飞星火 Chat Completion Service.
/// </summary>
public sealed class SparkChatService : IChatService
{
    private SparkChatServiceConfig? _config;

    /// <inheritdoc/>
    public IChatClient? Client { get; set; }

    /// <inheritdoc/>
    public AIServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig config)
    {
        if (config is not SparkChatServiceConfig hunyuanConfig)
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
            Endpoint = new Uri("https://spark-api-open.xf-yun.com/v1"),
        });

        Client?.Dispose();
        Client = coreClient.AsChatClient(_config.Model!);
    }

    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetPredefinedModels() =>
    [
        new("lite", "Spark Lite"),
        new("generalv3", "Spark Pro"),
        new("pro-128k", "Spark Pro 128K"),
        new("generalv3.5", "Spark Max", toolSupport : true),
        new("max-32k", "Spark Max 32K", toolSupport: true),
        new("4.0Ultra", "Spark 4.0 Ultra", toolSupport: true),
    ];
}
