// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.ZhiPu.Core;
using Richasy.AgentKernel.Connectors.ZhiPu.Models;
using Richasy.AgentKernel.Draw;
using Richasy.AgentKernel.Models;
using RichasyKernel;

namespace Richasy.AgentKernel.Connectors.ZhiPu;

/// <summary>
/// 智谱绘图服务.
/// </summary>
public sealed class ZhiPuDrawService : IDrawService
{
    private ZhiPuServiceConfig? _config;

    /// <inheritdoc/>
    public IDrawClient? Client { get; set; }

    /// <inheritdoc/>
    public AIServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig? config)
    {
        if (config is not ZhiPuServiceConfig aiConfig)
        {
            throw new KernelException("Configuration is invalid");
        }

        if (_config != null && aiConfig.Equals(_config))
        {
            return;
        }

        _config = aiConfig;
        Client?.Dispose();
        Client = new ZhiPuDrawClient(aiConfig.AccessKey, aiConfig.Model);
    }

    /// <inheritdoc/>
    public IReadOnlyList<DrawModel> GetPredefinedModels() =>
    [
        new ("cogview-4", "CogView-4", false, [new(1024,1024),new(768,1344),new(864,1152),new(1344,768),new(1152,864),new(1440,720),new(720,1440)]),
        new ("cogview-3-flash", "CogView-3 FLash", false, [new(1024,1024),new(768,1344),new(864,1152),new(1344,768),new(1152,864),new(1440,720),new(720,1440)]),
    ];
}
