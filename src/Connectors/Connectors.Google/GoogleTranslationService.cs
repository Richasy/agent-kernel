// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Google.Core;
using Richasy.AgentKernel.Connectors.Google.Models;
using Richasy.AgentKernel.Models;
using Richasy.AgentKernel.Translation;
using RichasyKernel;

namespace Richasy.AgentKernel.Connectors.Google;

/// <summary>
/// Google translate service.
/// </summary>
public sealed class GoogleTranslationService : ITranslationService
{
    private GoogleTranslationServiceConfig? _config;

    /// <inheritdoc/>
    public TranslationServiceConfig? Config => _config;

    /// <inheritdoc/>
    public ITranslateClient? Client { get; set; }

    /// <inheritdoc/>
    public void Initialize(TranslationServiceConfig config)
    {
        if (config is not GoogleTranslationServiceConfig googleConfig)
        {
            throw new KernelException("Configuration is invalid");
        }

        if (_config != null && googleConfig.Equals(_config))
        {
            return;
        }

        _config = googleConfig;
        Client?.Dispose();
        Client = new GoogleTranslateClient(googleConfig);
    }
}
