// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Audio;
using Richasy.AgentKernel.Connectors.Azure.Models;
using Richasy.AgentKernel.Connectors.Azure.Models.Audio;
using Richasy.AgentKernel.Models;
using RichasyKernel;
using System.Security;
using System.Text;

namespace Richasy.AgentKernel.Connectors.Azure.Core;

/// <summary>
/// Represents a client that can generate audio using Azure services.
/// </summary>
public sealed class AzureAudioClient(AzureAudioServiceConfig config) : IAudioClient
{
    private readonly string _apiEndpoint = $"https://{config.Region}.tts.speech.microsoft.com/cognitiveservices/v1";
    private readonly AzureAudioServiceConfig _config = config;
    private readonly HttpClient _httpClient = HttpExtensions.CreateHttpClient();

    /// <inheritdoc/>
    public AudioClientMetadata Metadata { get; } = new("azure", config.Region);

    /// <inheritdoc/>
    public void Dispose() => _httpClient?.Dispose();

    /// <inheritdoc/>
    public async Task<BinaryData> TextToSpeechAsync(string text, AudioOptions? options, CancellationToken cancellationToken = default)
    {
        var request = CreateHttpRequest(text, options);
        var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
        return new BinaryData(body, "audio/wav");
    }

    private HttpRequestMessage CreateHttpRequest(string text, AudioOptions? options)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, _apiEndpoint);
        request.Headers.Add("Ocp-Apim-Subscription-Key", _config.AccessKey);
        request.Headers.Add("User-Agent", "Agent-Kernel");
        request.Headers.Add("X-Microsoft-OutputFormat", "riff-24khz-16bit-mono-pcm");
        var azureOptions = options as AzureAudioOptions;
        var content = $"""
            <speak version='1.0' xml:lang='{options!.LanguageCode}'>
            <voice xml:lang='{options.LanguageCode}' xml:gender='{azureOptions?.Gender ?? string.Empty}' name='{options.VoiceId}'>
            <prosody rate='{options.Speed}'></prosody>
            {SecurityElement.Escape(text)}
            </voice>
            </speak>
            """;
        request.Content = new StringContent(content, Encoding.UTF8, "application/ssml+xml");
        return request;
    }
}
