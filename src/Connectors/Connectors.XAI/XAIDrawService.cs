// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.XAI.Core;
using Richasy.AgentKernel.Connectors.XAI.Models;
using Richasy.AgentKernel.Draw;
using Richasy.AgentKernel.Models;
using RichasyKernel;

namespace Richasy.AgentKernel.Connectors.XAI;

/// <summary>
/// xAI 绘图服务.
/// </summary>
public sealed class XAIDrawService : IDrawService
{
    private XAIServiceConfig? _config;

    /// <inheritdoc/>
    public IDrawClient? Client { get; set; }

    /// <inheritdoc/>
    public AIServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig? config)
    {
        if (config is not XAIServiceConfig aiConfig)
        {
            throw new KernelException("Configuration is invalid");
        }

        if ((_config != null && aiConfig.Equals(_config)) || string.IsNullOrEmpty(aiConfig?.Model))
        {
            return;
        }

        _config = aiConfig;
        Client?.Dispose();
        Client = new XAIDrawClient(aiConfig);
    }

    /// <inheritdoc/>
    public IReadOnlyList<DrawModel> GetPredefinedModels() =>
    [
        new ("grok-2-image", "Grok 2.0 Image", false, [new(1024,1024)]),
    ];
}
