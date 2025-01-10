// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Azure.Core;
using Richasy.AgentKernel.Connectors.Azure.Models;
using Richasy.AgentKernel.Models;
using Richasy.AgentKernel.Translation;
using RichasyKernel;

namespace Richasy.AgentKernel.Connectors.Azure;

/// <summary>
/// Azure 翻译服务.
/// </summary>
public sealed class AzureTranslationService : ITranslationService
{
    private AzureTranslationServiceConfig? _config;

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
        if (config is not AzureTranslationServiceConfig azureConfig)
        {
            throw new KernelException("Configuration is invalid");
        }

        if (_config != null && azureConfig.Equals(_config))
        {
            return;
        }

        _config = azureConfig;
        Client?.Dispose();
        Client = new AzureTranslateClient(azureConfig);
    }
}
