// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Core.OpenAI;
using Richasy.AgentKernel.Extensions.AI;
using Microsoft.Extensions.AI;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.Ali.Models;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Ali;

/// <summary>
/// 千问 Chat Completion Service.
/// </summary>
public sealed class QwenChatService : IChatService
{
    private QwenServiceConfig? _config;

    /// <inheritdoc/>
    public IChatClient? Client { get; set; }

    /// <inheritdoc/>
    public AIServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig config)
    {
        if (config is not QwenServiceConfig qwenConfig)
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
            Endpoint = new Uri("https://dashscope.aliyuncs.com/compatible-mode/v1"),
        });

        Client?.Dispose();
        Client = coreClient.AsChatClient(_config.Model!);
    }

    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetPredefinedModels()
    {
        return
        [
            new("qwen-max", "通义千问-Max", toolSupport: true),
            new("qwen-plus", "通义千问-Plus", toolSupport: true),
            new("qwen-turbo", "通义千问-Turbo", toolSupport: true),
            new("qwen-long", "Qwen-Long"),
            new("qwen-vl-max", "通义千问 VL", visionSupport: true),
            new("qwen2.5-72b-instruct", "通义千问 2.5-开源版"),
            new("qwen2-72b", "通义千问 2-开源版"),
            new("qwen1.5-110b-chat", "通义千问 1.5-开源版"),
            new("qwen-72b-chat", "通义千问 1-开源版"),
        ];
    }
}
