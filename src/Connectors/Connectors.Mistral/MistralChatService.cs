// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.Mistral.Core;
using Richasy.AgentKernel.Connectors.Mistral.Models;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Mistral;

/// <summary>
/// Mistral Chat Completion Service.
/// </summary>
public sealed class MistralChatService : IChatService
{
    private MistralServiceConfig? _config;

    /// <inheritdoc/>
    public IChatClient? Client { get; set; }

    /// <inheritdoc/>
    public AIServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig? config)
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
        Client?.Dispose();
        Client = new MistralChatClient(_config.AccessKey, _config.UseCodestralApi, _config.Model);
    }

    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetPredefinedModels() =>
    [
        new("mistral-large-latest", "Mistral Large", toolSupport: true),
        new("pixtral-large-latest", "Pixtral Large", toolSupport: true, visionSupport: true),
        new("ministral-3b-latest", "Ministral 3B"),
        new("ministral-8b-latest", "Ministral 8B"),
        new("ministral-small-latest", "Ministral Small"),
        new("codestral-latest", "Codestral"),
    ];
}
