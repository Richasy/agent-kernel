// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Translation;

/// <summary>
/// Represents a client that can translate content.
/// </summary>
public interface ITranslateClient : IDisposable
{
    /// <summary>
    /// Gets metadata that describes the <see cref="ITranslateClient"/>.
    /// </summary>
    public TranslateClientMetadata Metadata { get; }

    /// <summary>
    /// Translates the source content from the source language to the target language.
    /// </summary>
    /// <param name="sourceContent">The content to be translated.</param>
    /// <param name="options">The options for translation.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to monitor for cancellation requests. The default is <see cref="CancellationToken.None"/>.</param>
    /// <returns><see cref="TranslateCompletion"/>.</returns>
    Task<TranslateCompletion> TranslateTextAsync(
        string sourceContent,
        TranslateOptions? options,
        CancellationToken cancellationToken = default);

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
