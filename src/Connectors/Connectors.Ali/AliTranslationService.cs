// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using AlibabaCloud.SDK.Alimt20181012.Models;
using Richasy.AgentKernel.Connectors.Ali.Models;
using Richasy.AgentKernel.Models;
using Richasy.AgentKernel.Translation;
using RichasyKernel;

namespace Richasy.AgentKernel.Connectors.Ali;

/// <summary>
/// Ali Translation Service.
/// </summary>
public sealed class AliTranslationService : ITextTranslationService, IHtmlTranslationService
{
    private AlibabaCloud.SDK.Alimt20181012.Client? _client;
    private AliTranslationServiceConfig? _config;

    /// <inheritdoc/>
    public TranslationServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Dispose() => _client = default;

    /// <inheritdoc/>
    public void Initialize(TranslationServiceConfig config)
    {
        if (config is not AliTranslationServiceConfig aliConfig)
        {
            throw new KernelException("Configuration is invalid");
        }

        if (_config != null && aliConfig.Equals(_config))
        {
            return;
        }

        _config = aliConfig;
        var alibabaConfig = new AlibabaCloud.OpenApiClient.Models.Config
        {
            AccessKeyId = aliConfig.AccessKey,
            AccessKeySecret = aliConfig.Secret,
            Endpoint = "mt.aliyuncs.com",
        };

        _client = new AlibabaCloud.SDK.Alimt20181012.Client(alibabaConfig);
    }

    /// <inheritdoc/>
    public Task<TranslateCompletion> TranslateTextAsync(string sourceContent, TranslateOptions? options, CancellationToken cancellationToken = default)
        => TranslateInternalAsync(sourceContent, false, options, cancellationToken);

    /// <inheritdoc/>
    public Task<TranslateCompletion> TranslateHtmlAsync(string sourceContent, TranslateOptions? options, CancellationToken cancellationToken = default)
        => TranslateInternalAsync(sourceContent, true, options, cancellationToken);

    private async Task<TranslateCompletion> TranslateInternalAsync(string sourceContent, bool isHtml, TranslateOptions? options, CancellationToken cancellationToken)
    {
        if (_config == null)
        {
            throw new KernelException("Configuration is not initialized");
        }

        var text = sourceContent.Replace("\r", "\n", StringComparison.InvariantCultureIgnoreCase).Replace("\n\n", "\n", StringComparison.InvariantCultureIgnoreCase);
        var request = new TranslateGeneralRequest
        {
            FormatType = isHtml ? "html" : "text",
            SourceLanguage = options?.SourceLanguage ?? "auto",
            TargetLanguage = options?.TargetLanguage ?? "en",
            Scene = "general",
            SourceText = text,
        };

        var response = await _client!.TranslateGeneralAsync(request).ConfigureAwait(true);
        cancellationToken.ThrowIfCancellationRequested();
        if (response.Body is null)
        {
            throw new KernelException("Translation failed");
        }

        if (response.Body.Code != 200)
        {
            throw new KernelException($"Failed: {response.Body.Message}");
        }

        var res = response.Body;
        return new TranslateCompletion
        {
            Id = res!.RequestId,
            Result = res.Data!.Translated!,
            SourceContent = sourceContent,
            SourceLanguage = res.Data.DetectedLanguage,
            TargetLanguage = options?.TargetLanguage!,
        };
    }
}
