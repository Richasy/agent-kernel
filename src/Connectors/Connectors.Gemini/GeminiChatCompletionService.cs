// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using Richasy.AgentKernel.ChatCompletion;
using Richasy.AgentKernel.Connectors.Gemini.Models;

namespace Richasy.AgentKernel.Connectors.Gemini;

/// <summary>
/// Gemini Chat Completion Service.
/// </summary>
public sealed class GeminiChatCompletionService : IChatCompletionService
{
    private GeminiServiceConfig? _config;

    /// <inheritdoc/>
    public IChatClient? Client
    {
        get => field ?? throw new InvalidOperationException("The service has not been initialized.");
        set;
    }

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
        Client = new GeminiChatClient(_config.AccessKey, _config.Model, _config.Endpoint);
    }
}
