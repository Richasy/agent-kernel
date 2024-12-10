// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using Richasy.AgentKernel.ChatCompletion;
using Richasy.AgentKernel.Connectors.ZhiPu.Models;

namespace Richasy.AgentKernel.Connectors.ZhiPu;

/// <summary>
/// Chat completion service for ZhiPu.
/// </summary>
public sealed class ZhiPuChatCompletionService : IChatCompletionService
{
    private ZhiPuServiceConfig? _config;

    /// <inheritdoc/>
    public IChatClient? Client
    {
        get => field ?? throw new InvalidOperationException("The service has not been initialized.");
        set;
    }

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig config)
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
        Client = new ZhiPuChatClient(_config.AccessKey, _config.Model);
    }
}
