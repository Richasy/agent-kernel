// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Ali.Models;
using Richasy.AgentKernel.Connectors.Ali.Models.Translation;
using Richasy.AgentKernel.Models;
using Richasy.AgentKernel.Translation;
using RichasyKernel;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Richasy.AgentKernel.Connectors.Ali;

/// <summary>
/// Ali Translation Service.
/// </summary>
public sealed class AliTranslationService : ITextTranslationService, IHtmlTranslationService
{
    private const string _apiEndpoint = "http://mt.cn-hangzhou.aliyuncs.com/api/translate/web/general";
    private readonly HttpClient _httpClient;
    private AliTranslationServiceConfig? _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="AliTranslationService"/> class.
    /// </summary>
    public AliTranslationService()
        => _httpClient = HttpExtensions.CreateHttpClient();

    /// <inheritdoc/>
    public TranslationServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Dispose() => _httpClient?.Dispose();

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
        var request = new AliTranslateRequest()
        {
            FormatType = isHtml ? "html" : "text",
            SourceLanguage = options?.SourceLanguage ?? "auto",
            TargetLanguage = options?.TargetLanguage ?? "en",
            SourceText = text,
        };

        var requestJson = JsonSerializer.Serialize(request, JsonGenContext.Default.AliTranslateRequest);
        using var httpRequest = CreateRequest(_apiEndpoint, requestJson, _config.AccessKey, _config.Secret);
        using var response = await _httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var responseObj = JsonSerializer.Deserialize(responseContent, JsonGenContext.Default.AliTranslateResponse);

        return new TranslateCompletion
        {
            Id = responseObj!.RequestId,
            Result = responseObj!.Data!.Translated!,
            SourceContent = sourceContent,
            SourceLanguage = responseObj!.Data.DetectedLangauge,
            TargetLanguage = options?.TargetLanguage!,
        };
    }

    // 计算MD5并进行BASE64编码
    private static string? MD5Base64(string s)
    {
        if (s == null) return null;
        string encodeStr;
        var utfBytes = Encoding.UTF8.GetBytes(s);
#pragma warning disable CA5351 // 不要使用损坏的加密算法
        var md5Bytes = MD5.HashData(utfBytes);
#pragma warning restore CA5351 // 不要使用损坏的加密算法
        encodeStr = Convert.ToBase64String(md5Bytes);
        return encodeStr;
    }

    // 计算HMAC-SHA1
    private static string HMACSha1(string data, string key)
    {
#pragma warning disable CA5350 // 不要使用弱加密算法
        using var hmacsha1 = new HMACSHA1(Encoding.UTF8.GetBytes(key));
#pragma warning restore CA5350 // 不要使用弱加密算法
        var rawHmac = hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(data));
        return Convert.ToBase64String(rawHmac);
    }

    // 获取GMT时间
    private static string ToGMTString(DateTimeOffset date)
        => date.ToUniversalTime().ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'", System.Globalization.CultureInfo.InvariantCulture);

    // 发送POST请求
    private static HttpRequestMessage CreateRequest(string url, string body, string ak_id, string ak_secret)
    {
        var realUrl = new Uri(url);
        // HTTP header 参数
        var method = "POST";
        var accept = "application/json";
        var content_type = "application/json";
        var path = realUrl.AbsolutePath;
        var now = DateTimeOffset.Now;
        var date = ToGMTString(now);
        var host = realUrl.Host;
        // 1. 对body做MD5+BASE64加密
        var bodyMd5 = MD5Base64(body);
        var uuid = Guid.NewGuid().ToString();
        var stringToSign = method + "\n" + accept + "\n" + bodyMd5 + "\n" + content_type + "\n" + date + "\n"
            + "x-acs-action:TranslateGeneral\n"
            + "x-acs-signature-method:HMAC-SHA1\n"
                              + "x-acs-signature-nonce:" + uuid + "\n"
                              + "x-acs-signature-version:1.0\n"
                              + "x-acs-version:2018-10-12\n"
                              + path + "\n";
        // 2. 计算 HMAC-SHA1
        var signature = HMACSha1(stringToSign, ak_secret);
        // 3. 得到 authorization header
        var authHeader = "acs " + ak_id + ":" + signature;

        var request = new HttpRequestMessage(HttpMethod.Post, realUrl);
        request.Headers.TryAddWithoutValidation("Accept", accept);
        request.Headers.TryAddWithoutValidation("Content-Type", content_type);
        request.Headers.Date = now;
        request.Headers.TryAddWithoutValidation("Host", host);
        request.Headers.TryAddWithoutValidation("Authorization", authHeader);
        request.Headers.TryAddWithoutValidation("Content-MD5", bodyMd5);
        request.Headers.TryAddWithoutValidation("x-acs-action", "TranslateGeneral");
        request.Headers.TryAddWithoutValidation("x-acs-signature-nonce", uuid);
        request.Headers.TryAddWithoutValidation("x-acs-signature-method", "HMAC-SHA1");
        request.Headers.TryAddWithoutValidation("x-acs-version", "2018-10-12");
        request.Headers.TryAddWithoutValidation("Timestamp", now.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture));
        request.Content = new StringContent(body, Encoding.UTF8, "application/json");
        return request;
    }
}
