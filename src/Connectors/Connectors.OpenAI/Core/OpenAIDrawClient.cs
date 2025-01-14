// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.OpenAI.Models;
using Richasy.AgentKernel.Draw;
using Richasy.AgentKernel.Models;
using Richasy.AgentKernel.Core.OpenAI.Images;
using Richasy.AgentKernel.Core.OpenAI;

namespace Richasy.AgentKernel.Connectors.OpenAI.Core;

/// <summary>
/// Represents a draw client that uses OpenAI.
/// </summary>
public sealed class OpenAIDrawClient : IDrawClient
{
    private readonly OpenAIServiceConfig _config;
    private readonly ImageClient _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenAIDrawClient"/> class.
    /// </summary>
    public OpenAIDrawClient(OpenAIServiceConfig config)
    {
        _config = config;
        Metadata = new("openai", config.Model);
        var options = new OpenAIClientOptions();
        if (_config.Endpoint != null)
        {
            options.Endpoint = _config.Endpoint;
        }

        _client = new ImageClient(config.Model!, new(config.AccessKey), options);
    }

    /// <inheritdoc/>
    public DrawClientMetadata Metadata { get; }

    /// <inheritdoc/>
    public void Dispose()
    {
    }

    /// <inheritdoc/>
    public async Task<BinaryData> DrawAsync(string prompt, DrawOptions? options, CancellationToken cancellationToken = default)
    {
        var opt = new ImageGenerationOptions();
        if (options?.Width != null && options?.Height != null)
        {
            opt.Size = new GeneratedImageSize(options.Width.Value, options.Height.Value);
        }

        opt.Quality = GeneratedImageQuality.High;
        opt.ResponseFormat = GeneratedImageFormat.Bytes;
        var result = await _client.GenerateImageAsync(prompt, opt, cancellationToken).ConfigureAwait(false);
        return result.Value.ImageBytes;
    }
}
