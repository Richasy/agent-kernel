// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;
using Richasy.AgentKernel.Translation;

namespace Richasy.AgentKernel.Connectors.Google;

/// <summary>
/// Google translate service.
/// </summary>
public sealed class GoogleTranslationService : ITextTranslationService
{
    private const string _apiEndpoint = "https://translate.googleapis.com";
    private readonly HttpClient _httpClient;

    /// <inheritdoc/>
    public TranslationServiceConfig? Config => throw new NotImplementedException();
    /// <inheritdoc/>
    public void Dispose() => throw new NotImplementedException();
    /// <inheritdoc/>
    public void Initialize(TranslationServiceConfig config) => throw new NotImplementedException();
    /// <inheritdoc/>
    public Task<TranslateCompletion> TranslateTextAsync(string sourceContent, TranslateOptions? options, CancellationToken cancellationToken = default) => throw new NotImplementedException();
}
