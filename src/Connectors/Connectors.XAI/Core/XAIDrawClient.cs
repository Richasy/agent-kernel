// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.XAI.Models;
using Richasy.AgentKernel.Core.OpenAI;
using Richasy.AgentKernel.Core.OpenAI.Images;
using Richasy.AgentKernel.Draw;
using Richasy.AgentKernel.Models;
using RichasyKernel;

namespace Richasy.AgentKernel.Connectors.XAI.Core;

/// <summary>
/// xAI 绘图客户端.
/// </summary>
public sealed class XAIDrawClient : IDrawClient
{
    private readonly ImageClient _client;

    /// <summary>
    /// Initializes a new instance of the XAIDrawClient class with the provided service configuration.
    /// </summary>
    /// <param name="config">The configuration object contains necessary credentials and model information for the client.</param>
    public XAIDrawClient(XAIServiceConfig config)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(config.AccessKey, nameof(config.AccessKey));
        ArgumentException.ThrowIfNullOrWhiteSpace(config.Model, nameof(config.Model));
        Metadata = new("xai", config.Model);

        _client = new OpenAIClient(new(config.AccessKey), new OpenAIClientOptions
        {
            Endpoint = new Uri("https://api.xai.com/v1"),
        }).GetImageClient(config.Model!);
    }

    /// <inheritdoc/>
    public DrawClientMetadata Metadata { get; }

    /// <inheritdoc/>
    public void Dispose()
    {
        // Do nothing.
    }

    /// <inheritdoc/>
    public async Task<BinaryData> DrawAsync(string prompt, DrawOptions? options, CancellationToken cancellationToken = default)
    {
        var data = await _client.GenerateImageAsync(prompt, cancellationToken: cancellationToken).ConfigureAwait(false);
        if (data.Value?.ImageUri is null)
        {
            throw new InvalidOperationException("Failed to generate image.");
        }

        using var httpClient = new HttpClient();
        var imgResponse = await httpClient.GetAsync(data.Value.ImageUri, cancellationToken).ConfigureAwait(false);
        if (!imgResponse.IsSuccessStatusCode)
        {
            throw new KernelException($"Failed to get image data. {data.Value.ImageUri}");
        }

        var imgBytes = await imgResponse.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
        return new BinaryData(imgBytes, "image/jpeg");
    }
}
