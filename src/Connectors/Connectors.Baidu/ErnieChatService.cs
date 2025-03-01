// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.Baidu.Core;
using Richasy.AgentKernel.Connectors.Baidu.Models;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Baidu;

/// <summary>
/// 文心一言 Chat Completion Service.
/// </summary>
public sealed class ErnieChatService : IChatService
{
    private ErnieServiceConfig? _config;

    /// <inheritdoc/>
    public IChatClient? Client { get; set; }

    /// <inheritdoc/>
    public AIServiceConfig? Config => _config;

    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetPredefinedModels() =>
    [
        new("ernie-4.0-8k-latest", "ERNIE 4.0", toolSupport:true),
        new("ernie-4.0-turbo-8k-latest", "ERNIE 4.0 Turbo 8K", toolSupport:true),
        new("ernie-4.0-turbo-128k", "ERNIE 4.0 Turbo 128K", toolSupport:true),
        new("ernie-3.5-8k", "ERNIE 3.5 8K", toolSupport:true),
        new("ernie-3.5-128k", "ERNIE 3.5 128K", toolSupport:true),
        new("ernie-speed-8k", "ERNIE Speed 8K"),
        new("ernie-speed-128k", "ERNIE Speed 128K"),
        new("ernie-speed-pro-128k", "ERNIE Speed Pro 128K", toolSupport: true),
        new("ernie-lite-8k", "ERNIE Lite 8K"),
        new("ernie-lite-pro-128k", "ERNIE Lite Pro 128K", toolSupport: true),
        new("ernie-tiny-8k", "ERNIE Tiny"),
        new("ernie-character-8k", "ERNIE Character"),
        new("ernie-novel-8k", "ERNIE-Novel-8K"),
    ];

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig? config)
    {
        if (config is not ErnieServiceConfig ernieConfig)
        {
            throw new ArgumentException("The configuration is not valid.", nameof(config));
        }

        if (_config != null && ernieConfig.Equals(_config))
        {
            return;
        }

        _config = ernieConfig;
        Client?.Dispose();
        Client = new ErnieChatClient(ernieConfig);
    }
}
