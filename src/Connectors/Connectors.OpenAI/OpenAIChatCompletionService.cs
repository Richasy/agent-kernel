// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using OpenAI;
using Richasy.AgentKernel.ChatCompletion;
using Richasy.AgentKernel.Connectors.OpenAI.Models;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.OpenAI;

/// <summary>
/// OpenAI Chat Completion Service.
/// </summary>
public sealed class OpenAIChatCompletionService : IChatCompletionService
{
    private OpenAIServiceConfig? _config;

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
        if (config is not OpenAIServiceConfig oaiConfig)
        {
            throw new ArgumentException("The configuration is not valid.", nameof(config));
        }

        if (_config != null && oaiConfig.Equals(_config))
        {
            return;
        }

        _config = oaiConfig;
        var options = new OpenAIClientOptions();
        if (_config.Endpoint != null)
        {
            options.Endpoint = _config.Endpoint;
        }

        if (!string.IsNullOrEmpty(_config.Organization))
        {
            options.OrganizationId = _config.Organization;
        }

        var coreClient = new OpenAIClient(new(_config.AccessKey), options);
        Client = coreClient.AsChatClient(_config.Model!);
    }
}
