// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using Mistral.SDK;
using Richasy.AgentKernel.ChatCompletion;
using Richasy.AgentKernel.Connectors.Mistral.Models;

namespace Richasy.AgentKernel.Connectors.Mistral;

/// <summary>
/// Mistral Chat Completion Service.
/// </summary>
public sealed class MistralChatCompletionService : IChatCompletionService
{
    private MistralServiceConfig? _config;

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
        if (config is not MistralServiceConfig mistralConfig)
        {
            throw new ArgumentException("The configuration is not valid.", nameof(config));
        }

        if (_config != null && mistralConfig.Equals(_config))
        {
            return;
        }

        _config = mistralConfig;
        var c = new MistralClient(new APIAuthentication(_config.AccessKey));
        if (mistralConfig.UseCodestralApi)
        {
            c.ApiUrlFormat = "https://codestral.mistral.ai/{0}/{1}";
        }

        Client = c.Completions;
    }
}
