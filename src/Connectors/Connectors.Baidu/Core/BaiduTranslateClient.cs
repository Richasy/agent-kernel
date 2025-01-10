// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Baidu.Models;
using Richasy.AgentKernel.Models;
using Richasy.AgentKernel.Translation;
using RichasyKernel;
using System.Globalization;
using System.Text.Json;
using System.Text;

namespace Richasy.AgentKernel.Connectors.Baidu.Core;

/// <summary>
/// Baidu translation client.
/// </summary>
public sealed class BaiduTranslateClient(BaiduTranslationServiceConfig config) : ITranslateClient
{
    private const string _apiEndpoint = "https://fanyi-api.baidu.com/api/trans/vip/translate";
    private readonly HttpClient _httpClient = HttpExtensions.CreateHttpClient();
#pragma warning disable CA5394 // 请勿使用不安全的随机性
    private readonly string _salt = new Random().Next(100000).ToString(CultureInfo.InvariantCulture);
#pragma warning restore CA5394 // 请勿使用不安全的随机性
    private readonly BaiduTranslationServiceConfig? _config = config;

    /// <inheritdoc/>
    public TranslateClientMetadata Metadata { get; } = new("baidu", isTextSupported: true, isHtmlSupported: false);

    /// <inheritdoc/>
    public void Dispose() => _httpClient?.Dispose();

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

        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, _apiEndpoint)
        {
            Content = new FormUrlEncodedContent(queryList)
        };
        using var response = await _httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
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

    /// <inheritdoc/>
    public Task<TranslateCompletion> TranslateHtmlAsync(string sourceContent, TranslateOptions? options, CancellationToken cancellationToken = default) => throw new NotImplementedException();

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
