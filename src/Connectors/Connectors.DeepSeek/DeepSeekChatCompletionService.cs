// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Connectors.DeepSeek.Models;
using Microsoft.Extensions.AI;
using OpenAI;
using Richasy.AgentKernel.ChatCompletion;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.DeepSeek;

/// <summary>
/// DeepSeek Chat Completion Service.
/// </summary>
public sealed class DeepSeekChatCompletionService : IChatCompletionService
{
    private DeepSeekServiceConfig? _config;

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
        if (config is not DeepSeekServiceConfig deepseekConfig)
        {
            throw new ArgumentException("The configuration is not valid.", nameof(config));
        }

        if (_config != null && deepseekConfig.Equals(_config))
        {
            return;
        }

        _config = deepseekConfig;
        var coreClient = new OpenAIClient(new(_config.AccessKey), new OpenAIClientOptions
        {
            Endpoint = new Uri("https://api.deepseek.com"),
        });

        Client = coreClient.AsChatClient(_config.Model!);
    }
}
