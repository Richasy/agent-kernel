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
    public void Initialize(AIServiceConfig? config)
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
            new("qwen-flash", "通义千问-Flash", toolSupport: true),
            new("qwq-plus-latest", "QwQ 推理", toolSupport: true),
            new("qwen-omni-turbo", "通义千问-Omni"),
            new("qwen-long-latest", "通义千问-Long"),
            new("qwen-vl-max", "通义千问 VL", visionSupport: true),
            new("qwen3-next-80b-a3b-thinking", "通义千问 3-开源版-思考模式"),
            new("qwen3-next-80b-a3b-instruct", "通义千问 3-开源版-非思考模式"),
            new("qwen2.5-72b-instruct", "通义千问 2.5-开源版"),
            new("qwen2-72b", "通义千问 2-开源版"),
        ];
    }
}
