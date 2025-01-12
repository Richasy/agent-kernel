// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Draw;
using Richasy.AgentKernel.Connectors.OpenAI.Core;
using Richasy.AgentKernel.Connectors.OpenAI.Models;
using Richasy.AgentKernel.Models;
using RichasyKernel;

namespace Richasy.AgentKernel.Connectors.OpenAI;

/// <summary>
/// Represents an Draw service that uses OpenAI.
/// </summary>
public sealed class OpenAIDrawService : IDrawService
{
    private OpenAIServiceConfig? _config;

    /// <inheritdoc/>
    public IDrawClient? Client { get; set; }

    /// <inheritdoc/>
    public AIServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig? config)
    {
        if (config is not OpenAIServiceConfig aiConfig)
        {
            throw new KernelException("Configuration is invalid");
        }

        if (_config != null && aiConfig.Equals(_config))
        {
            return;
        }

        _config = aiConfig;
        Client?.Dispose();
        Client = new OpenAIDrawClient(aiConfig);
    }
}
