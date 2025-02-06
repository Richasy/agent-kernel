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

    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetPredefinedModels() =>
    [
        new("gemini-2.0-flash-exp", "Gemini 2.0 Flash", toolSupport: true, visionSupport: true),
        new("gemini-1.5-flash", "Gemini 1.5 Flash", toolSupport: true, visionSupport: true),
        new("gemini-1.5-flash-8b", "Gemini 1.5 Flash-8B", toolSupport: true, visionSupport: true),
        new("gemini-1.5-pro", "Gemini 1.5 Pro", toolSupport: true, visionSupport: true),
    ];
}
