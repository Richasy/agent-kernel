// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Google.Cloud.Translate.V3;
using Richasy.AgentKernel.Connectors.Google.Models;
using Richasy.AgentKernel.Models;
using Richasy.AgentKernel.Translation;
using RichasyKernel;

namespace Richasy.AgentKernel.Connectors.Google;

/// <summary>
/// Google translate service.
/// </summary>
public sealed class GoogleTranslationService : ITextTranslationService
{
    private GoogleTranslationServiceConfig? _config;
    private TranslationServiceClient? _client;

    /// <inheritdoc/>
    public TranslationServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Dispose() => _client = null;

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
        _client = new TranslationServiceClientBuilder
        {
            ApiKey = _config.AccessKey,
        }.Build();
    }

    /// <inheritdoc/>
    public async Task<TranslateCompletion> TranslateTextAsync(string sourceContent, TranslateOptions? options, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(sourceContent, nameof(sourceContent));
        if (_client is null)
        {
            throw new KernelException("The service is not initialized.");
        }

        var req = new TranslateTextRequest
        {
            Contents = { sourceContent },
            TargetLanguageCode = options?.TargetLanguage ?? "en-US",
        };
        var res = await _client.TranslateTextAsync(req).ConfigureAwait(false);
        var translation = res.Translations.FirstOrDefault();
        return new TranslateCompletion
        {
            Result = translation!.TranslatedText,
            SourceContent = sourceContent,
            SourceLanguage = translation.DetectedLanguageCode,
            TargetLanguage = options?.TargetLanguage,
        };
    }
}
