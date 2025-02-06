// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.IFlyTek.Core;
using Richasy.AgentKernel.Connectors.IFlyTek.Models;
using Richasy.AgentKernel.Draw;
using Richasy.AgentKernel.Models;
using RichasyKernel;

namespace Richasy.AgentKernel.Connectors.IFlyTek;

/// <summary>
/// Represents an Draw service that uses Spark.
/// </summary>
public sealed class SparkDrawService : IDrawService
{
    private SparkDrawServiceConfig? _config;

    /// <inheritdoc/>
    public IDrawClient? Client { get; set; }

    /// <inheritdoc/>
    public AIServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig? config)
    {
        if (config is not SparkDrawServiceConfig aiConfig)
        {
            throw new KernelException("Configuration is invalid");
        }

        if (_config != null && aiConfig.Equals(_config))
        {
            return;
        }

        _config = aiConfig;
        Client?.Dispose();
        Client = new SparkDrawClient(aiConfig);
    }

    /// <inheritdoc/>
    public IReadOnlyList<DrawModel> GetPredefinedModels() =>
    [
        new("2.1", "V2.1", false, [new(512,512), new(640,360), new(640,480), new(640,640), new(640,640), new(680,512), new(512,680), new(768,768), new(720,1280), new(1280,720), new(1024,1024)])
    ];
}
