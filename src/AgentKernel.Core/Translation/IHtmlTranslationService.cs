// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Translation;

/// <summary>
/// Provides methods to translate HTML content.
/// </summary>
public interface IHtmlTranslationService : IDisposable
{
    /// <summary>
    /// Translates the source content from the source language to the target language.
    /// </summary>
    /// <param name="sourceContent">The content to be translated.</param>
    /// <param name="options">The options for translation.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to monitor for cancellation requests. The default is <see cref="CancellationToken.None"/>.</param>
    /// <returns><see cref="TranslateCompletion"/>.</returns>
    Task<TranslateCompletion> TranslateHtmlAsync(
        string sourceContent,
        TranslateOptions? options,
        CancellationToken cancellationToken = default);
}
