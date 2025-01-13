// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Baidu.Core;
using Richasy.AgentKernel.Connectors.Baidu.Models;
using Richasy.AgentKernel.Models;
using Richasy.AgentKernel.Translation;
using RichasyKernel;

namespace Richasy.AgentKernel.Connectors.Baidu;

/// <summary>
/// Baidu translation service.
/// </summary>
public sealed class BaiduTranslationService : ITranslationService
{
    private BaiduTranslationServiceConfig? _config;

    /// <inheritdoc/>
    public TranslationServiceConfig? Config => _config;

    /// <inheritdoc/>
    public ITranslateClient? Client { get; set; }

    /// <inheritdoc/>
    public void Initialize(TranslationServiceConfig? config)
    {
        if (config is not BaiduTranslationServiceConfig baiduConfig)
        {
            throw new KernelException("Configuration is invalid");
        }

        if (_config != null && baiduConfig.Equals(_config))
        {
            return;
        }

        _config = baiduConfig;
        Client?.Dispose();
        Client = new BaiduTranslateClient(baiduConfig);
    }
}
