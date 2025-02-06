// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Core.OpenAI;
using Richasy.AgentKernel.Extensions.AI;
using Connectors.DeepSeek.Models;
using Microsoft.Extensions.AI;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.DeepSeek;

/// <summary>
/// DeepSeek Chat Completion Service.
/// </summary>
public sealed class DeepSeekChatService : IChatService
{
    private DeepSeekServiceConfig? _config;

    /// <inheritdoc/>
    public IChatClient? Client { get; set; }

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

        Client?.Dispose();
        Client = coreClient.AsChatClient(_config.Model!);
    }

    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetPredefinedModels() =>
    [
        new("deepseek-chat", "DeepSeek Chat"),
        new("deepseek-coder", "DeepSeek Coder"),
        new("deepseek-reasoner", "DeepSeek Reasoner"),
    ];
}
