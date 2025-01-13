// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Google.Core;
using Richasy.AgentKernel.Models;
using Richasy.AgentKernel.Translation;

namespace Richasy.AgentKernel.Connectors.Google;

/// <summary>
/// Google translate service.
/// </summary>
public sealed class GoogleTranslationService : ITranslationService
{
    /// <inheritdoc/>
    public TranslationServiceConfig? Config => null;

    /// <inheritdoc/>
    public ITranslateClient? Client { get; set; }

    /// <inheritdoc/>
    public void Initialize(TranslationServiceConfig? config)
    {
        if (Client is not null)
        {
            return;
        }

        Client = new GoogleTranslateClient();
    }
}
