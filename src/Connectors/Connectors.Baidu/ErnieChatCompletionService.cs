// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using Richasy.AgentKernel.ChatCompletion;
using Richasy.AgentKernel.Connectors.Baidu.Core;
using Richasy.AgentKernel.Connectors.Baidu.Models;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Baidu;

/// <summary>
/// 文心一言 Chat Completion Service.
/// </summary>
public sealed class ErnieChatCompletionService : IChatCompletionService
{
    private ErnieServiceConfig? _config;

    /// <inheritdoc/>
    public IChatClient? Client { get; set; }

    /// <inheritdoc/>
    public AIServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig config)
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
        Client = new ErnieChatClient(ernieConfig.AccessKey, ernieConfig.SecretKey, ernieConfig.Model);
    }
}
