// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Volcano.Core;
using Richasy.AgentKernel.Connectors.Volcano.Models;
using Richasy.AgentKernel.Models;
using Richasy.AgentKernel.Translation;
using RichasyKernel;

namespace Richasy.AgentKernel.Connectors.Volcano;

/// <summary>
/// Translation service that uses the Volcano translation service.
/// </summary>
public sealed class VolcanoTranslationService : ITranslationService
{
    private VolcanoTranslationServiceConfig? _config;

    /// <inheritdoc/>
    public TranslationServiceConfig? Config => _config;

    /// <inheritdoc/>
    public ITranslateClient? Client { get; set; }

    /// <inheritdoc/>
    public void Initialize(TranslationServiceConfig? config)
    {
        if (config is not VolcanoTranslationServiceConfig volcanoConfig)
        {
            throw new KernelException("Configuration is invalid");
        }

        if (_config != null && volcanoConfig.Equals(_config))
        {
            return;
        }

        _config = volcanoConfig;
        Client?.Dispose();
        Client = new VolcanoTranslateClient(volcanoConfig);
    }
}
