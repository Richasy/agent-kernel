// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Baidu.Core;
using Richasy.AgentKernel.Connectors.Baidu.Models;
using Richasy.AgentKernel.Draw;
using Richasy.AgentKernel.Models;
using RichasyKernel;

namespace Richasy.AgentKernel.Connectors.Baidu;

/// <summary>
/// Represents an Draw service that uses Ernie.
/// </summary>
public sealed class ErnieDrawService : IDrawService
{
    private ErnieServiceConfig? _config;

    /// <inheritdoc/>
    public IDrawClient? Client { get; set; }

    /// <inheritdoc/>
    public AIServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig? config)
    {
        if (config is not ErnieServiceConfig aiConfig)
        {
            throw new KernelException("Configuration is invalid");
        }

        if (_config != null && aiConfig.Equals(_config))
        {
            return;
        }

        _config = aiConfig;
        Client?.Dispose();
        Client = new ErnieDrawClient(aiConfig);
    }

    /// <inheritdoc/>
    public IReadOnlyList<DrawModel> GetPredefinedModels() =>
    [
        new("basic", "基础版", false, [new(512,512), new(640,360), new(360,640), new(1024,1024), new(720,1280), new(1280,720)]),
        new("ernievilg", "高级版", false, [new(512,512), new(640,360), new(360,640), new(1024,1024), new(720,1280), new(1280,720), new(2048,2048), new(2560,1440), new(1440,2560), new(3840,2160), new(2160,3840)]),
        new("extreme", "极速版", false, [new(512,512), new(640,360), new(360,640), new(1024,1024), new(720,1280), new(1280,720)])
    ];
}
