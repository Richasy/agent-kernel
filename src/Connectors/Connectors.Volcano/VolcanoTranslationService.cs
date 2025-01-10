// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Volcano.Core;
using Richasy.AgentKernel.Connectors.Volcano.Models;
using Richasy.AgentKernel.Connectors.Volcano.Models.Translation;
using Richasy.AgentKernel.Models;

// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Translation;
using RichasyKernel;
using System.Text.Json;

namespace Richasy.AgentKernel.Connectors.Volcano;

/// <summary>
/// Translation service that uses the Volcano translation service.
/// </summary>
public sealed class VolcanoTranslationService : ITranslationService
{
    private const string _apiEndpoint = "https://translate.volcengineapi.com";
    private const string _version = "2020-06-01";
    private const string _action = "TranslateText";
    private readonly HttpClient _httpClient;
    private VolcanoTranslationServiceConfig? _config;

    /// <summary>
    /// Initialize a new instance of <see cref="VolcanoTranslationService"/>.
    /// </summary>
    public VolcanoTranslationService()
        => _httpClient = HttpExtensions.CreateHttpClient();

    /// <inheritdoc/>
    public TranslationServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Dispose()
        => _httpClient?.Dispose();

    /// <inheritdoc/>
    public void Initialize(TranslationServiceConfig config)
    {
        if (config is not VolcanoTranslationServiceConfig volcanoConfig)
        {
            throw new KernelException("Configuration is invalid");
        }

        if (_config != null && volcanoConfig.Equals(_config))
        {
            return;
        }

        _config = volcanoConfig;
    }

    /// <inheritdoc/>
    public async Task<TranslateCompletion> TranslateTextAsync(string sourceContent, TranslateOptions? options, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(sourceContent, nameof(sourceContent));
        var text = sourceContent
            .Replace("\r", "\n", StringComparison.InvariantCultureIgnoreCase)
            .Replace("\n\n", "\n", StringComparison.InvariantCultureIgnoreCase);
        var endpoint = $"{_apiEndpoint}?Action={_action}&Version={_version}";
        var req = new VolcanoTranslateRequest
        {
            SourceLanguage = options?.SourceLanguage,
            TargetLanguage = options?.TargetLanguage ?? "en",
            TextList = [text],
        };
        var reqJson = JsonSerializer.Serialize(req, JsonGenContext.Default.VolcanoTranslateRequest);
        using var request = AuthorizeTool.CreateAuthorizedRequest(new(endpoint), reqJson, _config!.SecretId, _config!.AccessKey);
        using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new KernelException("Translation failed.", new HttpRequestException(content));
        }

        var responseObj = JsonSerializer.Deserialize(content, JsonGenContext.Default.VolcanoTranslateResponse);
        if (responseObj?.TranslationList is null)
        {
            throw new KernelException("Translation failed.");
        }

        var result = responseObj.TranslationList.FirstOrDefault();
        return new TranslateCompletion
        {
            Result = result!.Translation!,
            SourceContent = text,
            SourceLanguage = result.DetectedSourceLanguage,
            TargetLanguage = options?.TargetLanguage,
        };
    }
}
