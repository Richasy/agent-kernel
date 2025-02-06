// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Azure;
using Microsoft.Extensions.AI;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.Azure.Models;
using Richasy.AgentKernel.Core.AzureInference;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Azure;

/// <summary>
/// Azure AI Foundry Chat Service.
/// </summary>
public sealed class AzureAIChatService : IChatService
{
    private AzureOpenAIServiceConfig? _config;

    /// <inheritdoc/>
    public IChatClient? Client { get; set; }

    /// <inheritdoc/>
    public AIServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig config)
    {
        if (config is not AzureOpenAIServiceConfig azureConfig)
        {
            throw new ArgumentException("The configuration is not valid.", nameof(config));
        }

        if (_config != null && azureConfig.Equals(_config))
        {
            return;
        }

        _config = azureConfig;
        var client = new ChatCompletionsClient(_config.Endpoint, new AzureKeyCredential(_config.AccessKey));
        Client?.Dispose();
        Client = client.AsChatClient(config.Model);
    }

    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetPredefinedModels() => [];
}
