// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Translation;

/// <summary>
/// Provides methods to translate text.
/// </summary>
public interface ITranslateService
{
    /// <summary>
    /// Gets the client that is used to translate content.
    /// </summary>
    ITranslateClient? Client { get; }

    /// <summary>
    /// Gets the configuration of the translation service.
    /// </summary>
    TranslateServiceConfig? Config { get; }

    /// <summary>
    /// Initialize the translation service.
    /// </summary>
    /// <param name="config">Configuration.</param>
    void Initialize(TranslateServiceConfig? config);

    /// <summary>
    /// Gets the supported languages for translation.
    /// </summary>
    /// <returns><see cref="TranslationLanguages"/>.</returns>
    TranslationLanguages GetSupportedLanguages();
}
