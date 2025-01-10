// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Tencent.Core;
using Richasy.AgentKernel.Connectors.Tencent.Models;
using Richasy.AgentKernel.Models;
using Richasy.AgentKernel.Translation;
using RichasyKernel;

namespace Richasy.AgentKernel.Connectors.Tencent;

/// <summary>
/// Tencent Translation Service.
/// </summary>
public sealed class TencentTranslationService : ITranslationService
{
    private TencentTranslationServiceConfig? _config;

    /// <inheritdoc/>
    public TranslationServiceConfig? Config => _config;

    /// <inheritdoc/>
    public ITranslateClient? Client { get; set; }

    /// <inheritdoc/>
    public void Initialize(TranslationServiceConfig config)
    {
        if (config is not TencentTranslationServiceConfig tencentConfig)
        {
            throw new KernelException("Configuration is invalid");
        }

        if (_config != null && tencentConfig.Equals(_config))
        {
            return;
        }

        _config = tencentConfig;
        Client?.Dispose();
        Client = new TencentTranslateClient(tencentConfig);
    }
}
