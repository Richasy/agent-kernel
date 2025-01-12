// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using OpenAI;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.Groq.Models;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Groq;

/// <summary>
/// Groq Chat Completion Service.
/// </summary>
public sealed class GroqChatService : IChatService
{
    private GroqServiceConfig? _config;

    /// <inheritdoc/>
    public IChatClient? Client { get; set; }

    /// <inheritdoc/>
    public AIServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig config)
    {
        if (config is not GroqServiceConfig groqConfig)
        {
            throw new ArgumentException("The configuration is not valid.", nameof(config));
        }

        if (_config != null && groqConfig.Equals(_config))
        {
            return;
        }

        _config = groqConfig;
        var coreClient = new OpenAIClient(new(_config.AccessKey), new OpenAIClientOptions
        {
            Endpoint = new Uri("https://api.groq.com/openai/v1"),
        });

        Client?.Dispose();
        Client = coreClient.AsChatClient(_config.Model!);
    }
}
