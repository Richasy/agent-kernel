// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.ZhiPu.Core;
using Richasy.AgentKernel.Connectors.ZhiPu.Models;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.ZhiPu;

/// <summary>
/// Chat completion service for ZhiPu.
/// </summary>
public sealed class ZhiPuChatService : IChatService
{
    private ZhiPuServiceConfig? _config;

    /// <inheritdoc/>
    public IChatClient? Client { get; set; }

    /// <inheritdoc/>
    public AIServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig? config)
    {
        if (config is not ZhiPuServiceConfig zhipuConfig)
        {
            throw new ArgumentException("The configuration is not valid.", nameof(config));
        }

        if (_config != null && zhipuConfig.Equals(_config))
        {
            return;
        }

        _config = zhipuConfig;
        Client?.Dispose();
        Client = new ZhiPuChatClient(_config.AccessKey, _config.Model);
    }

    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetPredefinedModels() =>
    [
        new("glm-zero-preview", "GLM-Zero-Preview", toolSupport: true),
        new("glm-4-plus", "GLM-4-Plus", toolSupport: true),
        new("glm-4-0520", "GLM-4", toolSupport: true),
        new("glm-4-long", "GLM-4-Long", toolSupport: true),
        new("glm-4-airx", "GLM-4-AirX", toolSupport: true),
        new("glm-4-air", "GLM-4-Air", toolSupport: true),
        new("glm-4-flashx", "GLM-4-FlashX", toolSupport: true),
        new("glm-4-flash", "GLM-4-Flash", toolSupport: true),
        new("glm-4v-flash", "GLM-4V-Flash", visionSupport: true),
        new("glm-4v-plus", "GLM-4V-Plus", visionSupport: true),
        new("glm-4v", "GLM-4V", visionSupport: true),
    ];
}
