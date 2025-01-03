// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using Richasy.AgentKernel.ChatCompletion;
using Richasy.AgentKernel.Connectors.AzureOpenAI.Models;
using Richasy.AgentKernel.Models;
using System.ClientModel;

namespace Richasy.AgentKernel.Connectors.AzureOpenAI;

/// <summary>
/// Azure Chat Completion Service.
/// </summary>
public sealed class AzureChatCompletionService : IChatCompletionService
{
    private AzureOpenAIServiceConfig? _config;

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
        Client = coreClient.AsChatClient(_config.Model!);
    }
}
