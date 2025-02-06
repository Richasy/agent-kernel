// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Core.OpenAI;
using Richasy.AgentKernel.Extensions.AI;
using Microsoft.Extensions.AI;
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

    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetPredefinedModels() =>
    [
        new("gemma2-9b-it", "Gemma2 9B"),
        new("llama-3.3-70b-versatile", "Llama 3.3 70B"),
        new("llama-3.1-8b-instant", "Llama 3.1 8B"),
        new("llama3-70b-8192","Llama3 70B 8192"),
        new("mixtral-8x7b-32768","Mixtral 8x7B 32768"),
    ];
}
