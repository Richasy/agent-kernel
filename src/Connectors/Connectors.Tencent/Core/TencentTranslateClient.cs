// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Tencent.Models;
using Richasy.AgentKernel.Connectors.Tencent.Models.Translation;
using Richasy.AgentKernel.Models;
using Richasy.AgentKernel.Translation;
using RichasyKernel;
using System.Text.Json;

namespace Richasy.AgentKernel.Connectors.Tencent.Core;

/// <summary>
/// Tencent translation client.
/// </summary>
public sealed partial class TencentTranslateClient(TencentTranslationServiceConfig config) : ITranslateClient
{
    private const string _apiEndpoint = "https://tmt.tencentcloudapi.com";
    private const string _action = "TextTranslate";
    private readonly HttpClient _httpClient = HttpExtensions.CreateHttpClient();
    private readonly TencentTranslationServiceConfig? _config = config;

    /// <inheritdoc/>
    public TranslateClientMetadata Metadata { get; } = new("tencent", isTextSupported: true, isHtmlSupported: false);

    /// <inheritdoc/>
    public void Dispose()
        => _httpClient?.Dispose();

    /// <inheritdoc/>
    public Task<TranslateCompletion> TranslateHtmlAsync(string sourceContent, TranslateOptions? options, CancellationToken cancellationToken = default) => throw new NotImplementedException();

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
        return responseObj?.Response is null
            ? throw new KernelException("Translation failed.")
            : responseObj.Response.Error != null
            ? throw new KernelException($"{responseObj.Response.Error.Code}: {responseObj.Response.Error.Message}")
            : new TranslateCompletion
            {
                Result = responseObj.Response.TargetText!,
                Id = responseObj.Response.RequestId,
                SourceContent = text,
                SourceLanguage = responseObj.Response.Source,
                TargetLanguage = responseObj.Response.Target,
            };
    }
}
