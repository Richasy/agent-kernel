// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Translation;

/// <summary>
/// Provides methods to translate text.
/// </summary>
public interface ITranslationService
{
    /// <summary>
    /// Gets the client that is used to translate content.
    /// </summary>
    ITranslateClient? Client { get; }

    /// <summary>
    /// Gets the configuration of the translation service.
    /// </summary>
    TranslationServiceConfig? Config { get; }

    /// <summary>
    /// Initialize the translation service.
    /// </summary>
    /// <param name="config">Configuration.</param>
    void Initialize(TranslationServiceConfig config);
}
