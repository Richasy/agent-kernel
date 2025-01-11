// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Google.Models;
using Richasy.AgentKernel.Models;
using Richasy.AgentKernel.Translation;
using RichasyKernel;
using System.Net;
using System.Security.Authentication;
using System.Text.RegularExpressions;
using System.Web;

namespace Richasy.AgentKernel.Connectors.Google.Core;

/// <summary>
/// Google translate client.
/// </summary>
public sealed partial class GoogleTranslateClient : ITranslateClient
{
    private const string _apiEndpoint = "http://translate.google.com/m";
    private const string _userAgent = "Mozilla/4.0 (compatible;MSIE 6.0;Windows NT 5.1;SV1;.NET CLR 1.1.4322;.NET CLR 2.0.50727;.NET CLR 3.0.04506.30)";
    private readonly HttpClient _httpClient;
    private readonly GoogleTranslationServiceConfig? _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="GoogleTranslateClient"/> class.
    /// </summary>
    /// <param name="config"></param>
    public GoogleTranslateClient(GoogleTranslationServiceConfig config)
    {
        _config = config;
        var handler = new HttpClientHandler
        {
            AllowAutoRedirect = true,
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            SslProtocols = SslProtocols.None,
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true,
        };

        _httpClient = new HttpClient(handler, disposeHandler: true);
        _httpClient.DefaultRequestHeaders.Add("User-Agent", _userAgent);
    }

    /// <inheritdoc/>
    public TranslateClientMetadata Metadata { get; } = new("google", isTextSupported: true, isHtmlSupported: false);

    /// <inheritdoc/>
    public void Dispose() => _httpClient?.Dispose();
    
    /// <inheritdoc/>
    public async Task<TranslateCompletion> TranslateTextAsync(string sourceContent, TranslateOptions? options, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(sourceContent, nameof(sourceContent));
        var url = $"{_apiEndpoint}?tl={options!.TargetLanguage!}&sl={options.SourceLanguage ?? "auto"}&q={Uri.EscapeDataString(sourceContent)}";
        var response = await _httpClient.GetAsync(new Uri(url), cancellationToken).ConfigureAwait(false);
        var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var reResult = Regex.Matches(content, @"(?s)class=""(?:t0|result-container)"">(.*?)<");
        var targetLanguageResult = Regex.Match(content, @"<input[^>]*\bname=""hl""[^>]*\bvalue=""([^""]*)""");
        if (reResult.Count == 0)
        {
            throw new KernelException("Failed to translate text");
        }

        var targetLan = targetLanguageResult.Success ? targetLanguageResult.Groups[1].Value : options.TargetLanguage;
        var result = HttpUtility.HtmlDecode(reResult[0].Groups[1].Value);
        return new TranslateCompletion
        {
            SourceContent = sourceContent,
            Result = result,
            SourceLanguage = options.SourceLanguage,
            TargetLanguage = targetLan,
        };
    }

    /// <inheritdoc/>
    public Task<TranslateCompletion> TranslateHtmlAsync(string sourceContent, TranslateOptions? options, CancellationToken cancellationToken = default) => throw new NotImplementedException();
}
