// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Baidu.Models;
using Richasy.AgentKernel.Models;
using Richasy.AgentKernel.Translation;
using RichasyKernel;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace Richasy.AgentKernel.Connectors.Baidu;

/// <summary>
/// Baidu translation service.
/// </summary>
public sealed class BaiduTranslationService : ITranslationService
{
    private const string _apiEndpoint = "https://fanyi-api.baidu.com/api/trans/vip/translate";
    private readonly HttpClient _httpClient;
    private readonly string _salt;
    private BaiduTranslationServiceConfig? _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaiduTranslationService"/> class.
    /// </summary>
    public BaiduTranslationService()
    {
        _httpClient = HttpExtensions.CreateHttpClient();
#pragma warning disable CA5394 // 请勿使用不安全的随机性
        _salt = new Random().Next(100000).ToString(CultureInfo.InvariantCulture);
#pragma warning restore CA5394 // 请勿使用不安全的随机性
    }

    /// <inheritdoc/>
    public TranslationServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Dispose() => _httpClient?.Dispose();

    /// <inheritdoc/>
    public void Initialize(TranslationServiceConfig config)
    {
        if (config is not BaiduTranslationServiceConfig baiduConfig)
        {
            throw new KernelException("Configuration is invalid");
        }

        if (_config != null && baiduConfig.Equals(_config))
        {
            return;
        }

        _config = baiduConfig;
    }

    /// <inheritdoc/>
    public async Task<TranslateCompletion> TranslateTextAsync(string sourceContent, TranslateOptions? options, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(sourceContent, nameof(sourceContent));
        var queryList = new Dictionary<string, string>
        {
            { "q", sourceContent },
            { "from", options?.SourceLanguage ?? "auto" },
            { "to", options?.TargetLanguage ?? "en" },
            { "appid", _config?.AccessKey ?? string.Empty },
            { "salt", _salt },
            { "sign", GenerateSign(sourceContent) },
        };

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, _apiEndpoint);
        requestMessage.Content = new FormUrlEncodedContent(queryList);
        var response = await _httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new KernelException("Failed to translate text.", new HttpRequestException(responseContent));
        }

        var responseObj = JsonSerializer.Deserialize(responseContent, JsonGenContext.Default.BaiduTranslateResponse);
        if (responseObj?.Result is null)
        {
            throw new KernelException("Translation result is invalid.");
        }

        var result = responseObj.Result.FirstOrDefault();
        return new TranslateCompletion
        {
            SourceContent = sourceContent,
            Result = result!.Result!,
            SourceLanguage = responseObj.From,
            TargetLanguage = responseObj.To,
        };
    }

    private string GenerateSign(string input)
    {
        var byteOld = Encoding.UTF8.GetBytes(_config!.AccessKey + input + _salt + _config.Secret);
#pragma warning disable CA5351 // 不要使用损坏的加密算法
        var byteNew = System.Security.Cryptography.MD5.HashData(byteOld);
        var sign = new StringBuilder();
        foreach (var t in byteNew)
        {
            sign.Append(t.ToString("x2", CultureInfo.InvariantCulture));
        }

        return sign.ToString();
    }
}
