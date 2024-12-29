// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using OpenAI;
using Richasy.AgentKernel.ChatCompletion;
using Richasy.AgentKernel.Connectors.Ali.Models;

namespace Richasy.AgentKernel.Connectors.Ali;

/// <summary>
/// 千问 Chat Completion Service.
/// </summary>
public sealed class QwenChatCompletionService : IChatCompletionService
{
    private QwenServiceConfig? _config;

    /// <inheritdoc/>
    public IChatClient? Client
    {
        get => field ?? throw new InvalidOperationException("The service has not been initialized.");
        set;
    }

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

        Client = coreClient.AsChatClient(_config.Model);
    }
}
