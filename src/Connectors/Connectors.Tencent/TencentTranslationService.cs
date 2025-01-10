// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Tencent.Core;
using Richasy.AgentKernel.Connectors.Tencent.Models;
using Richasy.AgentKernel.Connectors.Tencent.Models.Translation;
using Richasy.AgentKernel.Models;
using Richasy.AgentKernel.Translation;
using RichasyKernel;
using System.Text.Json;

namespace Richasy.AgentKernel.Connectors.Tencent;

/// <summary>
/// Tencent Translation Service.
/// </summary>
public sealed class TencentTranslationService : ITranslationService
{
    private const string _apiEndpoint = "https://tmt.tencentcloudapi.com";
    private const string _action = "TextTranslate";
    private readonly HttpClient _httpClient;
    private TencentTranslationServiceConfig? _config;

    /// <summary>
    /// Initialize a new instance of <see cref="TencentTranslationService"/>.
    /// </summary>
    public TencentTranslationService()
        => _httpClient = HttpExtensions.CreateHttpClient();

    /// <inheritdoc/>
    public TranslationServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Dispose()
        => _httpClient?.Dispose();

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
    }

    /// <inheritdoc/>
    public async Task<TranslateCompletion> TranslateTextAsync(string sourceContent, TranslateOptions? options, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(sourceContent, nameof(sourceContent));
        var text = sourceContent
            .Replace("\r", "\n", StringComparison.InvariantCultureIgnoreCase)
            .Replace("\n\n", "\n", StringComparison.InvariantCultureIgnoreCase);
        var untranslatedText = options is TencentTranslateOptions tencentOptions ? tencentOptions.UntranslatedText : null;
        var req = new TencentTranslateRequest
        {
            SourceText = text,
            Source = options?.SourceLanguage ?? "auto",
            Target = options?.TargetLanguage ?? "en",
            ProjectId = 0,
            UntranslatedText = untranslatedText,
        };
        var reqJson = JsonSerializer.Serialize(req, JsonGenContext.Default.TencentTranslateRequest);
        using var request = AuthorizeTool.CreateAuthorizedRequest(new(_apiEndpoint), reqJson, _config!.SecretId, _config!.AccessKey, _action);
        using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new KernelException("Translation failed.", new HttpRequestException(content));
        }

        var responseObj = JsonSerializer.Deserialize(content, JsonGenContext.Default.TencentTranslateResponse);
        if (responseObj?.Response is null)
        {
            throw new KernelException("Translation failed.");
        }

        if (responseObj.Response.Error != null)
        {
            throw new KernelException($"{responseObj.Response.Error.Code}: {responseObj.Response.Error.Message}");
        }

        return new TranslateCompletion
        {
            Result = responseObj.Response.TargetText!,
            Id = responseObj.Response.RequestId,
            SourceContent = text,
            SourceLanguage = responseObj.Response.Source,
            TargetLanguage = responseObj.Response.Target,
        };
    }
}
