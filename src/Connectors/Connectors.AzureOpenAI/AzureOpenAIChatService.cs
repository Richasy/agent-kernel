// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Extensions.AI;
using Microsoft.Extensions.AI;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.Azure.Models;
using Richasy.AgentKernel.Models;
using System.ClientModel;
using Richasy.AgentKernel.Core.AzureOpenAI;

namespace Richasy.AgentKernel.Connectors.Azure;

/// <summary>
/// Azure Chat Completion Service.
/// </summary>
public sealed class AzureOpenAIChatService : IChatService
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
        var coreClient = new AzureOpenAIClient(_config.Endpoint, new ApiKeyCredential(_config.AccessKey));
        Client?.Dispose();
        Client = coreClient.AsChatClient(_config.Model!);
    }

    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetPredefinedModels() => [];
}
