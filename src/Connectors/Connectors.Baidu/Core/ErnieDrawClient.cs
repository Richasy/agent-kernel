// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Baidu.Models;
using Richasy.AgentKernel.Connectors.Baidu.Models.Ernie;
using Richasy.AgentKernel.Draw;
using Richasy.AgentKernel.Models;
using RichasyKernel;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace Richasy.AgentKernel.Connectors.Baidu.Core;

/// <summary>
/// 千帆绘图客户端.
/// </summary>
public sealed class ErnieDrawClient : IDrawClient
{
    private const string _apiEndpoint = "https://aip.baidubce.com/rpc/2.0/wenxin/v1/{0}/textToImage";
    private readonly HttpClient _httpClient;
    private readonly ErnieServiceConfig _config;
    private AccessToken? _token;
    private DateTimeOffset _expiry;

    /// <summary>
    /// Initializes a new instance of the <see cref="ErnieDrawClient"/> class.
    /// </summary>
    public ErnieDrawClient(ErnieServiceConfig config)
    {
        ArgumentException.ThrowIfNullOrEmpty(config.AccessKey, nameof(config.AccessKey));
        ArgumentException.ThrowIfNullOrEmpty(config.SecretKey, nameof(config.SecretKey));

        _config = config;
        _httpClient = HttpExtensions.CreateHttpClient();
        Metadata = new("ernie", config.Model);
    }

    /// <inheritdoc/>
    public DrawClientMetadata Metadata { get; }

    /// <inheritdoc/>
    public void Dispose() => _httpClient?.Dispose();

    /// <inheritdoc/>
    public async Task<BinaryData> DrawAsync(string prompt, DrawOptions? options, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(prompt, nameof(prompt));
        await EnsureAccessTokenAsync(cancellationToken).ConfigureAwait(false);
        var model = options?.ModelId ?? _config.Model;
        var endpoint = string.Format(CultureInfo.InvariantCulture, _apiEndpoint, model);
        var request = new HttpRequestMessage(HttpMethod.Post, $"{endpoint}?access_token={_token!.Token}");
        var requestObj = new ErnieDrawRequest
        {
            Text = prompt,
            Resolution = $"{options?.Width}*{options?.Height}",
        };

        if (options is ErnieDrawOptions edo)
        {
            requestObj.Style = edo.Style;
            requestObj.TextContent = edo.TextContent;
        }

        request.Content = new StringContent(JsonSerializer.Serialize(requestObj, JsonGenContext.Default.ErnieDrawRequest), Encoding.UTF8, "application/json");
        var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new KernelException($"Failed to draw image. {responseContent}");
        }

        var responseObj = JsonSerializer.Deserialize(responseContent, JsonGenContext.Default.ErnieDrawTaskResponse);
        if (!string.IsNullOrEmpty(responseObj?.ErrorMsg))
        {
            throw new KernelException($"Failed to draw image. {responseObj.ErrorMsg}");
        }

        var taskId = responseObj?.Data.TaskId;
        if (taskId is null or 0)
        {
            throw new KernelException("Failed to get task id.");
        }

        return await WaitUntilGetImageAsync(taskId.Value.ToString(CultureInfo.InvariantCulture), model!, cancellationToken).ConfigureAwait(false);
    }

    private async Task<BinaryData> WaitUntilGetImageAsync(string taskId, string model, CancellationToken cancellationToken)
    {
        BinaryData? result = null;

        do
        {
            var getImgEndpoint = $"https://aip.baidubce.com/rpc/2.0/wenxin/v1/{model}/getImg";
            await EnsureAccessTokenAsync(cancellationToken).ConfigureAwait(false);
            var request = new HttpRequestMessage(HttpMethod.Post, $"{getImgEndpoint}?access_token={_token!.Token}");
            var requestObj = new ErnieGetImageRequest
            {
                TaskId = taskId,
            };

            request.Content = new StringContent(JsonSerializer.Serialize(requestObj, JsonGenContext.Default.ErnieGetImageRequest), Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                throw new KernelException($"Failed to get image. {responseContent}");
            }

            var responseObj = JsonSerializer.Deserialize(responseContent, JsonGenContext.Default.ErnieGetImageResponse);
            if (responseObj?.Data is null)
            {
                throw new KernelException("Failed to get image data.");
            }

            var isFinish = responseObj.Data.Status == 1 && !string.IsNullOrWhiteSpace(responseObj.Data.Img);
            if (isFinish)
            {
                // Get image byte data from img url.
                var url = responseObj.Data.Img;
                var imgResponse = await _httpClient.GetAsync(new Uri(url!), cancellationToken).ConfigureAwait(false);
                if (!imgResponse.IsSuccessStatusCode)
                {
                    throw new KernelException($"Failed to get image data. {url}");
                }

                var imgBytes = await imgResponse.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
                result = new BinaryData(imgBytes, "image/jpeg");
            }
            else
            {
                await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
            }
        }
        while (result is null);

        return result;
    }

    private async Task EnsureAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        if (_token is not null && _expiry > DateTimeOffset.Now)
        {
            return;
        }

        _token = await AuthorizeTool.GenerateAccessTokenAsync(_config.AccessKey, _config.SecretKey, cancellationToken).ConfigureAwait(false);
        if (_token is null || string.IsNullOrEmpty(_token.Token))
        {
            throw new KernelException("Failed to get access token.");
        }

        _expiry = DateTimeOffset.Now.AddSeconds(Convert.ToInt32(_token.ExpiresIn, CultureInfo.InvariantCulture));
    }
}
