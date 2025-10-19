// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.Ollama.Core;
using Richasy.AgentKernel.Connectors.Ollama.Models;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Ollama;

/// <summary>
/// Ollama Chat Completion Service.
/// </summary>
public sealed class OllamaChatService : IChatService
{
    private OllamaServiceConfig? _config;

    /// <inheritdoc/>
    public Microsoft.Extensions.AI.IChatClient? Client { get; set; }

    /// <inheritdoc/>
    public AIServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig? config)
    {
        if (config is not OllamaServiceConfig ollamaConfig)
        {
            throw new ArgumentException("The configuration is not valid.", nameof(config));
        }

        if (_config != null && ollamaConfig.Equals(_config))
        {
            return;
        }

        _config = ollamaConfig;
        Client?.Dispose();
        var endpoint = ollamaConfig.Endpoint?.ToString() ?? "http://localhost:11434";
        if (!endpoint.EndsWith("/api", StringComparison.OrdinalIgnoreCase))
        {
            endpoint = endpoint.TrimEnd('/') + "/api";
        }

        Client = new OllamaChatClient(endpoint, ollamaConfig.Model);
    }

    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetPredefinedModels() => [];
}
