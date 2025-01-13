// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Youdao.Core;
using Richasy.AgentKernel.Connectors.Youdao.Models;
using Richasy.AgentKernel.Models;
using Richasy.AgentKernel.Translation;
using RichasyKernel;

namespace Richasy.AgentKernel.Connectors.Youdao;

/// <summary>
/// Youdao translation service.
/// </summary>
public sealed class YoudaoTranslationService : ITranslationService
{
    private YoudaoTranslationServiceConfig? _config;

    /// <inheritdoc/>
    public TranslationServiceConfig? Config => _config;

    /// <inheritdoc/>
    public ITranslateClient? Client { get; set; }

    /// <inheritdoc/>
    public void Initialize(TranslationServiceConfig? config)
    {
        if (config is not YoudaoTranslationServiceConfig youdaoConfig)
        {
            throw new KernelException("Configuration is invalid");
        }

        if (_config != null && youdaoConfig.Equals(_config))
        {
            return;
        }

        _config = youdaoConfig;
        Client?.Dispose();
        Client = new YoudaoTranslateClient(youdaoConfig);
    }
}
