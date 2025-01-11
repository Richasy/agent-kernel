// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Azure.Models;
using Richasy.AgentKernel.Connectors.Azure.Models.Translation;
using Richasy.AgentKernel.Models;
using Richasy.AgentKernel.Translation;
using RichasyKernel;
using System.Text.Json;
using System.Text;

namespace Richasy.AgentKernel.Connectors.Azure.Core;

/// <summary>
/// Azure Translation Client.
/// </summary>
public sealed partial class AzureTranslateClient(AzureTranslationServiceConfig config) : ITranslateClient
{
    private const string _apiEndpoint = "https://api.cognitive.microsofttranslator.com/translate";
    private const string _version = "3.0";

    private readonly HttpClient _httpClient = HttpExtensions.CreateHttpClient();
    private readonly AzureTranslationServiceConfig? _config = config;

    /// <inheritdoc/>
    public TranslateClientMetadata Metadata { get; } = new TranslateClientMetadata("azure", isTextSupported: true, isHtmlSupported: true);

    /// <inheritdoc/>
    public void Dispose() => _httpClient?.Dispose();

    /// <inheritdoc/>
    public Task<TranslateCompletion> TranslateHtmlAsync(string sourceContent, TranslateOptions? options, CancellationToken cancellationToken = default)
        => TranslateInternalAsync(sourceContent, true, options, cancellationToken);

    /// <inheritdoc/>
    public Task<TranslateCompletion> TranslateTextAsync(string sourceContent, TranslateOptions? options, CancellationToken cancellationToken = default)
        => TranslateInternalAsync(sourceContent, false, options, cancellationToken);

    private async Task<TranslateCompletion> TranslateInternalAsync(string sourceContent, bool isHtml, TranslateOptions? options, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(sourceContent, nameof(sourceContent));
        List<TranslationTextItem> requestObj = [new(sourceContent)];
        var request = CreateHttpRequest(options, isHtml);
        var requestJson = JsonSerializer.Serialize(requestObj, JsonGenContext.Default.ListTranslationTextItem);
        request.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");
        var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var responseJson = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new KernelException(responseJson);
        }

        var responseObj = JsonSerializer.Deserialize(responseJson, JsonGenContext.Default.ListTranslationResponse)
            ?? throw new KernelException("Response is null");
        ValidateResponse(responseObj);
        var result = ProcessTextTranslateResult(responseObj).First();
        result.SourceContent = sourceContent;
        return result;
    }

    private static List<TranslateCompletion> ProcessTextTranslateResult(List<TranslationResponse> response)
    {
        var result = new List<TranslateCompletion>();
        foreach (var item in response)
        {
            var firstTranslate = item.Translations?.FirstOrDefault();
            if (firstTranslate != null)
            {
                var t = new TranslateCompletion
                {
                    Result = firstTranslate.Text!,
                    SourceLanguage = item.DetectedLanguage?.Language,
                    TargetLanguage = firstTranslate.To,
                };
                result.Add(t);
            }
        }

        return result;
    }

    private static void ValidateResponse(List<TranslationResponse> response)
    {
        if (response == null || response.Count == 0)
        {
            throw new KernelException("Response is null or empty");
        }

        foreach (var item in response)
        {
            if (item.Translations == null || item.Translations.Count == 0)
            {
                throw new KernelException("Translation is null or empty");
            }
        }
    }

    private HttpRequestMessage CreateHttpRequest(TranslateOptions? options, bool isHtml)
    {
        var queryList = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(options?.SourceLanguage))
        {
            queryList.Add("from", options.SourceLanguage);
        }

        queryList.Add("to", options?.TargetLanguage ?? "en");
        queryList.Add("api-version", _version);
        queryList.Add("textType", isHtml ? "html" : "plain");

        var uriBuilder = new UriBuilder(_apiEndpoint)
        {
            Query = string.Join("&", queryList.Select(item => $"{item.Key}={item.Value}"))
        };

        var request = new HttpRequestMessage(HttpMethod.Post, uriBuilder.Uri);
        request.Headers.Add("Ocp-Apim-Subscription-Key", _config!.AccessKey);
        if (!string.IsNullOrEmpty(_config!.Region))
        {
            request.Headers.Add("Ocp-Apim-Subscription-Region", _config.Region);
        }

        return request;
    }
}
