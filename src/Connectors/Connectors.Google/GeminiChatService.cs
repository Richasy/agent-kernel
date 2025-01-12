// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.Google.Models;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Google;

/// <summary>
/// Gemini Chat Completion Service.
/// </summary>
public sealed class GeminiChatService : IChatService
{
    private GeminiServiceConfig? _config;

    /// <inheritdoc/>
    public IChatClient? Client { get; set; }

    /// <inheritdoc/>
    public AIServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig config)
    {
        if (config is not GeminiServiceConfig geminiConfig)
        {
            throw new ArgumentException("The configuration is not valid.", nameof(config));
        }

        if (_config != null && geminiConfig.Equals(_config))
        {
            return;
        }

        _config = geminiConfig;
        Client?.Dispose();
        Client = new GeminiChatClient(_config.AccessKey, _config.Model, _config.Endpoint);
    }
}
