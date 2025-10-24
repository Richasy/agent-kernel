// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Core.OpenAI;
using Richasy.AgentKernel.Extensions.AI;
using Microsoft.Extensions.AI;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.XAI.Models;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.XAI;

/// <summary>
/// XAI Chat Completion Service.
/// </summary>
public sealed class XAIChatService : IChatService
{
    private XAIServiceConfig? _config;

    /// <inheritdoc/>
    public IChatClient? Client { get; set; }

    /// <inheritdoc/>
    public AIServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig? config)
    {
        if (config is not XAIServiceConfig xaiConfig)
        {
            throw new ArgumentException("The configuration is not valid.", nameof(config));
        }

        if (_config != null && xaiConfig.Equals(_config))
        {
            return;
        }

        _config = xaiConfig;
        var coreClient = new OpenAIClient(new(_config.AccessKey), new OpenAIClientOptions
        {
            Endpoint = new Uri("https://api.x.ai/v1"),
        });

        Client?.Dispose();
        Client = coreClient.AsChatClient(_config.Model!);
    }

    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetPredefinedModels() =>
    [
        new("grok-4-fast-reasoning", "Grok 4 Fast Reasoning", toolSupport: true, visionSupport: true),
        new("grok-4-fast-non-reasoning", "Grok 4 Fast Non-Reasoning", toolSupport: true, visionSupport: true),
        new("grok-4-0709", "Grok 4", toolSupport: true, visionSupport: true),
        new("grok-3", "Grok 3", toolSupport: true),
        new("grok-3-mini", "Grok 3 Mini", toolSupport: true),
        new("grok-2-latest", "Grok 2", toolSupport: true),
        new("grok-2-vision", "Grok 2 Vision", toolSupport: true, visionSupport: true),
        new("grok-beta", "Grok Beta", toolSupport: true),
    ];
}
