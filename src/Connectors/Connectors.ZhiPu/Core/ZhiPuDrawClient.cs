// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.ZhiPu.Models;
using Richasy.AgentKernel.Draw;
using Richasy.AgentKernel.Models;
using RichasyKernel;
using System.Text;
using System.Text.Json;

namespace Richasy.AgentKernel.Connectors.ZhiPu.Core;

/// <summary>
/// 智谱绘图客户端.
/// </summary>
public sealed class ZhiPuDrawClient : IDrawClient
{
    private const string _apiEndpoint = "https://open.bigmodel.cn/api/paas/v4/images/generations";
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the ZhiPuDrawClient class with authorization and model metadata.
    /// </summary>
    public ZhiPuDrawClient(string accessKey, string? modelId)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(accessKey, nameof(accessKey));
        ArgumentNullException.ThrowIfNullOrWhiteSpace(modelId, nameof(modelId));

        _httpClient = HttpExtensions.CreateHttpClient();
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessKey);
        Metadata = new("zhipu", modelId);
    }

    /// <inheritdoc/>
    public DrawClientMetadata Metadata { get; }

    /// <inheritdoc/>
    public void Dispose() => _httpClient?.Dispose();

    /// <inheritdoc/>
    public async Task<BinaryData> DrawAsync(string prompt, DrawOptions? options, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(prompt, nameof(prompt));
        var req = new ZhiPuDrawRequest
        {
            Prompt = prompt,
            Size = $"{options?.Width ?? 1024}x{options?.Height ?? 1024}",
            Model = options?.ModelId ?? Metadata.ModelId ?? "cogview-4",
        };

        var reqJson = JsonSerializer.Serialize(req, JsonGenerationContext.Default.ZhiPuDrawRequest);
        using var httpResponse = await _httpClient.PostAsync(
            new Uri(_apiEndpoint),
            new StringContent(reqJson, Encoding.UTF8, "application/json"),
            cancellationToken).ConfigureAwait(false);

        var responseText = await httpResponse.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (responseText.StartsWith("{\"error\"", StringComparison.InvariantCultureIgnoreCase))
        {
            var errorResponse = JsonSerializer.Deserialize(responseText, JsonGenerationContext.Default.ZhiPuErrorResponse);
            throw new KernelException(errorResponse?.Error?.Message ?? "Unknown error");
        }

        var response = JsonSerializer.Deserialize(responseText, JsonGenerationContext.Default.ZhiPuDrawResponse)
            ?? throw new KernelException("Failed to parse ZhiPu Draw response.");
        var url = response.Data?.First().Url;
        if (string.IsNullOrEmpty(url))
        {
            var filter = response.ContentFilter?.FirstOrDefault();
            if (filter != null)
            {
                throw new KernelException($"Content filter occur. {filter.Role} | {filter.Level}");
            }

            throw new KernelException("Failed to get image data.");
        }

        var imgResponse = await _httpClient.GetAsync(new Uri(url!), cancellationToken).ConfigureAwait(false);
        if (!imgResponse.IsSuccessStatusCode)
        {
            throw new KernelException($"Failed to get image data. {url}");
        }

        var imgBytes = await imgResponse.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
        return new BinaryData(imgBytes, "image/png");
    }
}
