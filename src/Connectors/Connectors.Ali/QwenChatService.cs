// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using AgentKernel.Core.OpenAI;
using AgentKernel.Extensions.AI;
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
}
