// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Youdao.Models;
using Richasy.AgentKernel.Models;

// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Translation;
using RichasyKernel;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace Richasy.AgentKernel.Connectors.Youdao;

/// <summary>
/// Youdao translation service.
/// </summary>
public sealed class YoudaoTranslationService : ITranslationService
{
    private const string _apiEndpoint = "https://openapi.youdao.com/api";
    private readonly string _salt;
    private readonly HttpClient _httpClient;
    private YoudaoTranslationServiceConfig? _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="YoudaoTranslationService"/> class.
    /// </summary>
    public YoudaoTranslationService()
    {
        _salt = Guid.NewGuid().ToString("N");
        _httpClient = HttpExtensions.CreateHttpClient();
    }

    /// <inheritdoc/>
    public TranslationServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Dispose() => _httpClient?.Dispose();

    /// <inheritdoc/>
    public void Initialize(TranslationServiceConfig config)
    {
        if (config is not YoudaoTranslationServiceConfig youdaoConfig)
        {
            throw new KernelException("Configuration is invalid");
        }

        if (_config != null && youdaoConfig.Equals(_config))
        {
            return;
        }

        _config = youdaoConfig;
    }

    /// <inheritdoc/>
    public async Task<TranslateCompletion> TranslateTextAsync(string sourceContent, TranslateOptions? options, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(sourceContent, nameof(sourceContent));
        var queryList = new Dictionary<string, string>
        {
            { "q", sourceContent },
            { "from", options?.SourceLanguage ?? "auto" },
            { "to", options?.TargetLanguage! },
            { "appKey", _config!.AppId },
            { "salt", _salt },
            { "sign", GenerateSign(sourceContent, out var currentTime) },
            { "signType", "v3" },
            { "curtime", currentTime },
        };

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, _apiEndpoint)
        {
            Content = new FormUrlEncodedContent(queryList)
        };
        var response = await _httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new KernelException("Failed to translate text", new HttpRequestException(responseContent));
        }

        var result = JsonSerializer.Deserialize(responseContent, JsonGenContext.Default.YoudaoTranslateResult);
        if (result!.Translation is null || result!.Translation!.Length == 0)
        {
            throw new KernelException($"Failed to translate text: {result.ErrorCode}");
        }

        var langSplit = result.Language!.Split('2');
        return new TranslateCompletion
        {
            Result = result.Translation.First(),
            SourceLanguage = langSplit[0],
            TargetLanguage = langSplit[1],
            SourceContent = sourceContent,
        };
    }

    private string GenerateSign(string input, out string currentTime)
    {
        var q = input.Length > 20
            ? input[..10] + input.Length + input[^10..]
            : input;
        currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture);
        var sign = _config!.AppId + q + _salt + currentTime + _config!.AccessKey;
        var bytes = Encoding.UTF8.GetBytes(sign);
        var hashBytes = System.Security.Cryptography.SHA256.HashData(bytes);
        return Convert.ToHexStringLower(hashBytes);
    }
}
