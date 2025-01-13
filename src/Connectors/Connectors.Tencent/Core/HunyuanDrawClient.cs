// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Tencent.Models;
using Richasy.AgentKernel.Connectors.Tencent.Models.Draw;
using Richasy.AgentKernel.Draw;
using Richasy.AgentKernel.Models;
using RichasyKernel;
using System.Text.Json;

namespace Richasy.AgentKernel.Connectors.Tencent.Core;

/// <summary>
/// 混元绘图客户端.
/// </summary>
public sealed class HunyuanDrawClient : IDrawClient
{
    private const string _apiEndpoint = "https://hunyuan.tencentcloudapi.com";
    private readonly HttpClient _httpClient;
    private readonly HunyuanDrawServiceConfig _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="HunyuanDrawClient"/> class.
    /// </summary>
    public HunyuanDrawClient(HunyuanDrawServiceConfig config)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(config.AccessKey, nameof(config.AccessKey));
        ArgumentException.ThrowIfNullOrWhiteSpace(config.SecretId, nameof(config.SecretId));
        _config = config;
        _httpClient = new HttpClient();
        Metadata = new("hunyuan", config.Model);
    }

    /// <inheritdoc/>
    public DrawClientMetadata Metadata { get; }

    /// <inheritdoc/>
    public void Dispose() => _httpClient?.Dispose();

    /// <inheritdoc/>
    public async Task<BinaryData> DrawAsync(string prompt, DrawOptions? options, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(prompt, nameof(prompt));
        var req = new HunyuanDrawCreateRequest
        {
            Prompt = prompt,
            Resolution = $"{options?.Width ?? 512}:{options?.Height ?? 512}",
        };
        var model = options?.ModelId ?? _config.Model;
        var isLite = model == "lite";
        if (isLite)
        {
            req.RspImgType = "url";
        }

        var reqJson = JsonSerializer.Serialize(req, JsonGenContext.Default.HunyuanDrawCreateRequest);
        var action = isLite ? "TextToImageLite" : "SubmitHunyuanImageJob";
        using var request = AuthorizeTool.CreateAuthorizedRequest(new(_apiEndpoint), reqJson, _config.SecretId, _config.AccessKey, action, version: "2023-09-01", region: "ap-guangzhou");
        using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new KernelException("Draw failed.", new HttpRequestException(content));
        }

        if (isLite)
        {
            var responseObj = JsonSerializer.Deserialize(content, JsonGenContext.Default.HunyuanDrawLiteResponse);
            return string.IsNullOrEmpty(responseObj?.Response?.ResultImage)
                ? throw new KernelException("Failed to create hunyuan draw job.", new HttpRequestException(content))
                : await DownloadImageAsync(responseObj.Response.ResultImage!, cancellationToken).ConfigureAwait(false);
        }
        else
        {
            var responseObj = JsonSerializer.Deserialize(content, JsonGenContext.Default.HunyuanDrawCreateResponse);
            if (string.IsNullOrEmpty(responseObj?.Response?.JobId))
            {
                throw new KernelException("Failed to create hunyuan draw job.", new HttpRequestException(content));
            }

            var jobId = responseObj!.Response!.JobId;
            do
            {
                var (image, finished) = await this.GetJobResultAsync(jobId!, cancellationToken).ConfigureAwait(false);
                if (finished)
                {
                    return string.IsNullOrEmpty(image)
                        ? throw new KernelException("Failed to get image from HunYuan draw job.")
                        : await DownloadImageAsync(image, cancellationToken).ConfigureAwait(false);
                }

                await Task.Delay(2000, cancellationToken).ConfigureAwait(false);
            } while (!cancellationToken.IsCancellationRequested);
        }

        throw new KernelException("Draw job timeout.");
    }

    private async Task<BinaryData> DownloadImageAsync(string url, CancellationToken cancellationToken)
    {
        var imgResponse = await _httpClient.GetAsync(new Uri(url), cancellationToken).ConfigureAwait(false);
        if (!imgResponse.IsSuccessStatusCode)
        {
            throw new KernelException($"Failed to get image data. {url}");
        }

        var imgBytes = await imgResponse.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
        return new BinaryData(imgBytes, "image/png");
    }

    /// <summary>
    /// Get the result of a HunYuan image generation job.
    /// </summary>
    /// <returns>First parameter is image link, second parameter indicate the job was finished.</returns>
    private async Task<(string, bool)> GetJobResultAsync(string jobId, CancellationToken cancellationToken)
    {
        var req = new HunyuanDrawQueryRequest
        {
            JobId = jobId,
        };
        var reqJson = JsonSerializer.Serialize(req, JsonGenContext.Default.HunyuanDrawQueryRequest);
        using var request = AuthorizeTool.CreateAuthorizedRequest(new(_apiEndpoint), reqJson, _config.SecretId, _config.AccessKey, "QueryHunyuanImageJob", version: "2023-09-01", region: "ap-guangzhou");
        using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new KernelException("Query job failed.", new HttpRequestException(content));
        }

        var responseObj = JsonSerializer.Deserialize(content, JsonGenContext.Default.HunyuanDrawQueryResponse);
        // Task failed.
        if (response is null || responseObj!.Response!.JobStatusCode == "4")
        {
            return (string.Empty, true);
        }

        // Success.
        if (responseObj!.Response!.JobStatusCode == "5")
        {
            var image = responseObj!.Response!.ResultImage!.FirstOrDefault();
            return (image!, true);
        }

        return (string.Empty, false);
    }
}
