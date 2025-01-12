// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using OpenAI;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.Moonshot.Models;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Moonshot;

/// <summary>
/// Moonshot Chat Completion Service.
/// </summary>
public sealed class MoonshotChatService : IChatService
{
    private MoonshotServiceConfig? _config;

    /// <inheritdoc/>
    public IChatClient? Client { get; set; }

    /// <inheritdoc/>
    public AIServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig config)
    {
        if (config is not MoonshotServiceConfig moonshotConfig)
        {
            throw new ArgumentException("The configuration is not valid.", nameof(config));
        }

        if (_config != null && moonshotConfig.Equals(_config))
        {
            return;
        }

        _config = moonshotConfig;
        var coreClient = new OpenAIClient(new(_config.AccessKey), new OpenAIClientOptions
        {
            Endpoint = new Uri("https://api.moonshot.cn/v1"),
        });

        Client?.Dispose();
        Client = coreClient.AsChatClient(_config.Model!);
    }
}
