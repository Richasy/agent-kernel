// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using AgentKernel.Core.AzureOpenAI;
using AgentKernel.Core.OpenAI.Images;
using Richasy.AgentKernel.Connectors.Azure.Models;
using Richasy.AgentKernel.Draw;
using Richasy.AgentKernel.Models;
using System.ClientModel;

namespace Richasy.AgentKernel.Connectors.Azure.Core;

/// <summary>
/// Azure OpenAI 绘图客户端.
/// </summary>
public sealed class AzureOpenAIDrawClient : IDrawClient
{
    private readonly AzureOpenAIServiceConfig _config;
    private readonly AzureOpenAIClient _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="AzureOpenAIDrawClient"/> class.
    /// </summary>
    public AzureOpenAIDrawClient(AzureOpenAIServiceConfig config)
    {
        _config = config;
        Metadata = new("azure_openai", config.Model);
        _client = new AzureOpenAIClient(_config.Endpoint, new ApiKeyCredential(_config.AccessKey));
    }

    /// <inheritdoc/>
    public DrawClientMetadata Metadata { get; }

    /// <inheritdoc/>
    public void Dispose() { }

    /// <inheritdoc/>
    public async Task<BinaryData> DrawAsync(string prompt, DrawOptions? options, CancellationToken cancellationToken = default)
    {
        var model = options?.ModelId ?? _config.Model;
        var drawClient = _client.GetImageClient(model);
        var opt = new ImageGenerationOptions();
        if (options?.Width != null && options?.Height != null)
        {
            opt.Size = new GeneratedImageSize(options.Width.Value, options.Height.Value);
        }

        opt.Quality = GeneratedImageQuality.High;
        opt.ResponseFormat = GeneratedImageFormat.Bytes;
        var result = await drawClient.GenerateImageAsync(prompt, opt, cancellationToken).ConfigureAwait(false);
        return result.Value.ImageBytes;
    }
}
