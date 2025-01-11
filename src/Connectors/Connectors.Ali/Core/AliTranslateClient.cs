// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Ali.Models;
using Richasy.AgentKernel.Models;
using Richasy.AgentKernel.Translation;
using RichasyKernel;
using System.Globalization;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text;

namespace Richasy.AgentKernel.Connectors.Ali.Core;

/// <summary>
/// Client for translating content using the Ali translation service.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="AliTranslateClient"/> class.
/// </remarks>
public sealed partial class AliTranslateClient(AliTranslationServiceConfig config) : ITranslateClient
{
    private const string _apiEndpoint = "http://mt.cn-hangzhou.aliyuncs.com/api/translate/web/general";
    private readonly HttpClient _httpClient = HttpExtensions.CreateHttpClient();
    private readonly AliTranslationServiceConfig? _config = config;

    /// <inheritdoc/>
    public TranslateClientMetadata Metadata { get; } = new("ali", isTextSupported: true, isHtmlSupported: true);

    /// <inheritdoc/>
    public void Dispose() => _httpClient?.Dispose();

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

        var dict = new Dictionary<string, string>
        {
            { "FormatType", isHtml ? "html" : "text" },
            { "SourceText",  text},
            { "SourceLanguage", options?.SourceLanguage ?? "auto" },
            { "TargetLanguage", options?.TargetLanguage ?? "en" },
        };

        using var httpRequest = CreateRequest(_apiEndpoint, dict, _config.AccessKey, _config.Secret);
        using var response = await _httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var responseObj = JsonSerializer.Deserialize(responseContent, JsonGenContext.Default.AliTranslateResponse);

        return new TranslateCompletion
        {
            Id = responseObj!.RequestId,
            Result = responseObj!.Data!.Translated!,
            SourceContent = sourceContent,
            SourceLanguage = responseObj!.Data.DetectedLangauge ?? options?.TargetLanguage,
            TargetLanguage = options?.TargetLanguage!,
        };
    }

    // 计算MD5并进行BASE64编码
    private static string? ComputeHashedPayload(string payload)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(payload));
        return Convert.ToHexStringLower(bytes);
    }

    // HMAC-SHA256加密
    private static string HMACSha256(string data, string key)
    {
        var encoding = new UTF8Encoding();
        var keyByte = encoding.GetBytes(key);
        var dataBytes = encoding.GetBytes(data);
        var hash = HMACSHA256.HashData(keyByte, dataBytes);
        var hex = new StringBuilder(hash.Length * 2);
        foreach (var b in hash)
        {
            hex.AppendFormat(CultureInfo.InvariantCulture, "{0:x2}", b);
        }

        return hex.ToString();
    }

    // 获取GMT时间
    private static string ToGMTString(DateTimeOffset date)
        => date.ToUniversalTime().ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'", System.Globalization.CultureInfo.InvariantCulture);

    // 发送POST请求
    private static HttpRequestMessage CreateRequest(string url, Dictionary<string, string> formData, string ak_id, string ak_secret)
    {
        var realUrl = new Uri(url);
        // HTTP header 参数
        const string method = "POST";
        const string accept = "application/json";
        const string content_type = "application/x-www-form-urlencoded";
        var now = DateTimeOffset.Now;
        var date = ToGMTString(now);
        var host = realUrl.Host;
        var path = realUrl.AbsolutePath;
        // Convert formData to string.
        var body = string.Join("&", formData.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"));
        // 1. 对body做MD5+BASE64加密
        var hashedBody = ComputeHashedPayload(body);
        var uuid = Guid.NewGuid().ToString();

        var canonicalHeaders =
            $"content-type:{content_type}\n" +
            $"host:{host}\n" +
            "x-acs-action:TranslateGeneral\n" +
            $"x-acs-content-sha256:{hashedBody}\n" +
            $"x-acs-date:{now.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture)}\n" +
            $"x-acs-signature-nonce:{uuid}\n" +
            "x-acs-version:2018-10-12\n";
        const string signedHeaders = "content-type;host;x-acs-action;x-acs-content-sha256;x-acs-date;x-acs-signature-nonce;x-acs-version";
        var canonicalRequest =
            method + "\n" +
            path + "\n\n" +
            canonicalHeaders + "\n" +
            signedHeaders + "\n" +
            hashedBody;
        var hashedCanonicalRequest = ComputeHashedPayload(canonicalRequest);
        var stringToSign =
            "ACS3-HMAC-SHA256\n" + hashedCanonicalRequest;
        // 2. 计算 HMAC-SHA1
        var signature = HMACSha256(stringToSign, ak_secret);
        // 3. 得到 authorization header
        var authHeader = $"ACS3-HMAC-SHA256 Credential={ak_id},SignedHeaders={signedHeaders},Signature={signature}";

        var request = new HttpRequestMessage(HttpMethod.Post, realUrl);
        request.Headers.TryAddWithoutValidation("Accept", accept);
        request.Headers.Date = now.ToUniversalTime();
        request.Headers.TryAddWithoutValidation("Host", host);
        request.Headers.TryAddWithoutValidation("Authorization", authHeader);
        request.Headers.TryAddWithoutValidation("x-acs-content-sha256", hashedBody);
        request.Headers.TryAddWithoutValidation("x-acs-action", "TranslateGeneral");
        request.Headers.TryAddWithoutValidation("x-acs-signature-nonce", uuid);
        request.Headers.TryAddWithoutValidation("x-acs-version", "2018-10-12");
        request.Headers.TryAddWithoutValidation("x-acs-date", now.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture));
        request.Content = new FormUrlEncodedContent(formData);
        return request;
    }
}
