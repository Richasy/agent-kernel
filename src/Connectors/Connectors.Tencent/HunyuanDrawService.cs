// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Tencent.Core;
using Richasy.AgentKernel.Connectors.Tencent.Models;
using Richasy.AgentKernel.Draw;
using Richasy.AgentKernel.Models;
using RichasyKernel;

namespace Richasy.AgentKernel.Connectors.Tencent;

/// <summary>
/// Represents an Draw service that uses Hunyuan.
/// </summary>
public sealed class HunyuanDrawService : IDrawService
{
    private HunyuanDrawServiceConfig? _config;

    /// <inheritdoc/>
    public IDrawClient? Client { get; set; }

    /// <inheritdoc/>
    public AIServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig? config)
    {
        if (config is not HunyuanDrawServiceConfig aiConfig)
        {
            throw new KernelException("Configuration is invalid");
        }

        if (_config != null && aiConfig.Equals(_config))
        {
            return;
        }

        _config = aiConfig;
        Client?.Dispose();
        Client = new HunyuanDrawClient(aiConfig);
    }

    /// <inheritdoc/>
    public IReadOnlyList<DrawModel> GetPredefinedModels() =>
    [
        new("lite", "轻量版", true, [new(768,768), new(768,1024), new(1024,768), new(1024,1024), new(720,1280), new(1280,720), new(768,1280),new(1280,768),new(1080,1920),new(1920,1080)]),
        new("standard", "标准版", true, [new(768,768), new(768,1024), new(1024,768), new(1024,1024), new(720,1280), new(1280,720), new(768,1280),new(1280,768)]),
    ];
}
