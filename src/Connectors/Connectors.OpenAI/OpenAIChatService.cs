// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Core.OpenAI;
using Richasy.AgentKernel.Extensions.AI;
using Microsoft.Extensions.AI;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.OpenAI.Models;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.OpenAI;

/// <summary>
/// OpenAI Chat Completion Service.
/// </summary>
public sealed class OpenAIChatService : IChatService
{
    private OpenAIServiceConfig? _config;

    /// <inheritdoc/>
    public IChatClient? Client { get; set; }

    /// <inheritdoc/>
    public AIServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig? config)
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
        Client?.Dispose();
        Client = coreClient.AsChatClient(_config.Model!);
    }

    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetPredefinedModels() =>
    [
        new("gpt-4.1", "GPT-4.1", toolSupport: true, visionSupport: true),
        new("gpt-4o", "GPT-4o", toolSupport: true, visionSupport: true),
        new("gpt-4o-mini", "GPT-4o Mini", toolSupport: true, visionSupport: true),
        new("gpt-4.5-preview", "GPT-4.5 Preview", toolSupport: true, visionSupport: true),
        new("o4-mini", "O4 Mini", toolSupport: true, visionSupport: true),
        new("o3", "O3", toolSupport: true, visionSupport: true),
        new("o3-mini", "O3 Mini", toolSupport: true),
        new("o1", "O1", toolSupport: true, visionSupport: true),
        new("o1-mini", "O1 Mini", toolSupport: true, visionSupport: true),
        new("o1-preview", "O1 Preview", toolSupport: true, visionSupport: true),
        new("gpt-4", "GPT-4", toolSupport: true),
        new("gpt-3.5-turbo", "GPT-3.5 Turbo"),
    ];
}
