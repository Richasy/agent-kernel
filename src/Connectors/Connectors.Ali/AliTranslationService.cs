// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Ali.Core;
using Richasy.AgentKernel.Connectors.Ali.Models;
using Richasy.AgentKernel.Models;
using Richasy.AgentKernel.Translation;
using RichasyKernel;

namespace Richasy.AgentKernel.Connectors.Ali;

/// <summary>
/// Ali Translation Service.
/// </summary>
public sealed class AliTranslationService : ITranslationService
{
    private AliTranslationServiceConfig? _config;

    /// <inheritdoc/>
    public TranslationServiceConfig? Config => _config;

    /// <inheritdoc/>
    public ITranslateClient? Client
    {
        get => field ?? throw new InvalidOperationException("The service has not been initialized.");
        set;
    }

    /// <inheritdoc/>
    public void Initialize(TranslationServiceConfig config)
    {
        if (config is not AliTranslationServiceConfig aliConfig)
        {
            throw new KernelException("Configuration is invalid");
        }

        if (_config != null && aliConfig.Equals(_config))
        {
            return;
        }

        _config = aliConfig;
        Client?.Dispose();
        Client = new AliTranslateClient(aliConfig);
    }
}
