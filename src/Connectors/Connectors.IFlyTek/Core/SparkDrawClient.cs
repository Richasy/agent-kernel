// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using Richasy.AgentKernel.Connectors.IFlyTek.Models;
using Richasy.AgentKernel.Connectors.IFlyTek.Models.Draw;
using Richasy.AgentKernel.Draw;
using Richasy.AgentKernel.Models;
using RichasyKernel;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Richasy.AgentKernel.Connectors.IFlyTek.Core;

/// <summary>
/// The client for the Spark draw service.
/// </summary>
public sealed class SparkDrawClient : IDrawClient
{
    private const string _apiEndpoint = "https://spark-api.cn-huabei-1.xf-yun.com/v2.1/tti";
    private readonly HttpClient _httpClient;
    private readonly SparkDrawServiceConfig _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="SparkDrawClient"/> class.
    /// </summary>
    public SparkDrawClient(SparkDrawServiceConfig config)
    {
        ArgumentException.ThrowIfNullOrEmpty(config.AccessKey, nameof(config.AccessKey));
        ArgumentException.ThrowIfNullOrEmpty(config.Secret, nameof(config.Secret));
        ArgumentException.ThrowIfNullOrWhiteSpace(config.AppId, nameof(config.AppId));

        _config = config;
        _httpClient = HttpExtensions.CreateHttpClient();
        Metadata = new("spark", config.Model);
    }

    /// <inheritdoc/>
    public DrawClientMetadata Metadata { get; }

    /// <inheritdoc/>
    public void Dispose() => _httpClient?.Dispose();

    /// <inheritdoc/>
    public async Task<BinaryData> DrawAsync(string prompt, DrawOptions? options, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(prompt, nameof(prompt));
        var req = GetDrawRequest(prompt, options);
        var endpoint = GetAuthorizationUrl(_config.AccessKey, _config.Secret, _apiEndpoint);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, endpoint)
        {
            Content = new StringContent(JsonSerializer.Serialize(req, JsonGenContext.Default.SparkDrawRequest), Encoding.UTF8, "application/json"),
        };
        using var response = await _httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to draw image: {content}");
        }

        var responseObj = JsonSerializer.Deserialize(content, JsonGenContext.Default.SparkTextResponse);
        if (responseObj?.Header?.Code != 0)
        {
            throw new KernelException($"Failed to generate image: {responseObj?.Header?.Message}");
        }

        var base64Image = responseObj.Payload?.Choices?.Text?.FirstOrDefault()?.Content ?? string.Empty;
        return string.IsNullOrEmpty(base64Image)
            ? throw new KernelException("Failed to generate image: empty response")
            : new BinaryData(Convert.FromBase64String(base64Image), "image/png");
    }

    private SparkDrawRequest GetDrawRequest(string prompt, DrawOptions? options)
    {
        return new SparkDrawRequest
        {
            Header = new SparkRequestHeader
            {
                AppId = _config.AppId,
            },

            Parameter = new SparkDrawRequest.SparkDrawRequestParametersContainer
            {
                Image = new SparkDrawRequest.SparkDrawRequestParameters
                {
                    Domain = _config.Model ?? "general",
                    Width = options?.Width ?? 512,
                    Height = options?.Height ?? 512,
                },
            },

            Payload = new SparkBasicRequestPayload
            {
                Message = new SparkMessage
                {
                    Text =
                    [
                        new()
                        {
                            Role = ChatRole.User,
                            Content = prompt,
                        }
                    ],
                },
            },
        };
    }

    private static string GetAuthorizationUrl(string apiKey, string secret, string authUrl, string type = "POST")
    {
        var url = new Uri(authUrl);
        var dateString = DateTime.UtcNow.ToString("r");
        var signatureBytes = Encoding.ASCII.GetBytes($"host: {url.Host}\ndate: {dateString}\n{type} {url.AbsolutePath} HTTP/1.1");

        using HMACSHA256 hmacsha256 = new(Encoding.ASCII.GetBytes(secret));
        var computedHash = hmacsha256.ComputeHash(signatureBytes);
        var signature = Convert.ToBase64String(computedHash);

        var authorizationString = $"api_key=\"{apiKey}\",algorithm=\"hmac-sha256\",headers=\"host date request-line\",signature=\"{signature}\"";
        var authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes(authorizationString));

        var query = $"authorization={authorization}&date={dateString}&host={url.Host}";

        return new UriBuilder(url) { Scheme = url.Scheme, Query = query }.ToString();
    }
}
