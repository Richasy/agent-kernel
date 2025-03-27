// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Audio;
using Richasy.AgentKernel.Connectors.Volcano.Models;
using Richasy.AgentKernel.Connectors.Volcano.Models.Audio;
using Richasy.AgentKernel.Models;
using RichasyKernel;
using System.Text;
using System.Text.Json;

namespace Richasy.AgentKernel.Connectors.Volcano.Core;

/// <summary>
/// 火山AI音频客户端.
/// </summary>
public sealed class VolcanoAudioClient : IAudioClient
{
    private const string _apiEndpoint = "https://openspeech.bytedance.com/api/v1/tts";
    private readonly VolcanoAudioServiceConfig _config;
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the audio client for the volcano service.
    /// </summary>
    /// <param name="config">Provides configuration settings necessary for the audio client to function properly.</param>
    public VolcanoAudioClient(VolcanoAudioServiceConfig config)
    {
        _config = config;
        Metadata = new AudioClientMetadata("volcano", config.Model);
        _httpClient = HttpExtensions.CreateHttpClient();
    }

    /// <inheritdoc/>
    public AudioClientMetadata Metadata { get; }

    /// <inheritdoc/>
    public void Dispose()
        => _httpClient?.Dispose();

    /// <inheritdoc/>
    public async Task<BinaryData> TextToSpeechAsync(string text, AudioOptions? options, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(text, nameof(text));
        var textType = text.StartsWith("<speak", StringComparison.InvariantCultureIgnoreCase) ? "ssml" : "plain";
        var req = new VolcanoAudioRequest
        {
            App = new VolcanoAudioRequest.AppData
            {
                AppId = _config.AppId,
                Token = _config.AccessKey,
            },
            User = new(),
            Audio = new VolcanoAudioRequest.AudioData
            {
                Speed = options?.Speed ?? 1d,
                VoiceId = options?.VoiceId,
            },
            Request = new VolcanoAudioRequest.RequestData
            {
                Text = text,
                TextType = textType,
            }
        };

        var reqJson = JsonSerializer.Serialize(req, JsonGenContext.Default.VolcanoAudioRequest);
        using var request = new HttpRequestMessage(HttpMethod.Post, _apiEndpoint);
        request.Headers.TryAddWithoutValidation("Authorization", $"Bearer;{_config.AccessKey}");
        request.Content = new StringContent(reqJson, Encoding.UTF8, "application/json");
        using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        var result = JsonSerializer.Deserialize(content, JsonGenContext.Default.VolcanoAudioResponse)
            ?? throw new KernelException("服务器返回空结果");
        if (result.Code != 3000 || string.IsNullOrEmpty(result.Data))
        {
            throw new KernelException("服务器返回错误结果", new HttpRequestException(result.Message));
        }

        var data = Convert.FromBase64String(result.Data!);
        return BinaryData.FromBytes(data, "audio/wav");
    }
}
