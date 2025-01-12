// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using OpenAI;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.LingYi.Models;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.LingYi;

/// <summary>
/// LingYi Chat Completion Service.
/// </summary>
public sealed class LingYiChatService : IChatService
{
    private LingYiServiceConfig? _config;

    /// <inheritdoc/>
    public IChatClient? Client { get; set; }

    /// <inheritdoc/>
    public AIServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig config)
    {
        if (config is not LingYiServiceConfig lingYiConfig)
        {
            throw new ArgumentException("The configuration is not valid.", nameof(config));
        }

        if (_config != null && lingYiConfig.Equals(_config))
        {
            return;
        }

        _config = lingYiConfig;
        var coreClient = new OpenAIClient(new(_config.AccessKey), new OpenAIClientOptions
        {
            Endpoint = new Uri("https://api.lingyiwanwu.com/v1"),
        });

        Client?.Dispose();
        Client = coreClient.AsChatClient(_config.Model!);
    }
}
