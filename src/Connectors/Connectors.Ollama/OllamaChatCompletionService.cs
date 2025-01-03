// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using Richasy.AgentKernel.ChatCompletion;
using Richasy.AgentKernel.Connectors.Ollama.Models;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Ollama;

/// <summary>
/// Ollama Chat Completion Service.
/// </summary>
public sealed class OllamaChatCompletionService : IChatCompletionService
{
    private OllamaServiceConfig? _config;

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
        if (config is not OllamaServiceConfig ollamaConfig)
        {
            throw new ArgumentException("The configuration is not valid.", nameof(config));
        }

        if (_config != null && ollamaConfig.Equals(_config))
        {
            return;
        }

        _config = ollamaConfig;
        Client = new OllamaChatClient(_config.Endpoint!, _config.Model);
    }
}
