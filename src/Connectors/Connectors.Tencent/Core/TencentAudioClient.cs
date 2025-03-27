// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Audio;
using Richasy.AgentKernel.Connectors.Tencent.Models;
using Richasy.AgentKernel.Connectors.Tencent.Models.Audio;
using Richasy.AgentKernel.Models;
using RichasyKernel;
using System.Globalization;
using System.Text.Json;

namespace Richasy.AgentKernel.Connectors.Tencent.Core;

/// <summary>
/// 腾讯TTS客户端.
/// </summary>
public sealed class TencentAudioClient : IAudioClient
{
    private const string _apiEndpoint = "https://tts.tencentcloudapi.com";
    private const string _action = "TextToVoice";
    private readonly TencentAudioServiceConfig? _config;
    private readonly HttpClient _httpClient = HttpExtensions.CreateHttpClient();

    /// <summary>
    /// Initializes a TencentAudioClient instance with the specified configuration.
    /// </summary>
    /// <param name="config">The configuration provides necessary settings for the audio service.</param>
    public TencentAudioClient(TencentAudioServiceConfig config)
    {
        _config = config;
        Metadata = new AudioClientMetadata("tencent", config.Model);
    }

    /// <inheritdoc/>
    public AudioClientMetadata Metadata { get; }

    /// <inheritdoc/>
    public void Dispose()
        => _httpClient?.Dispose();

    /// <inheritdoc/>
    public async Task<BinaryData> TextToSpeechAsync(string text, AudioOptions? options, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(text, nameof(text));
        var req = new TencentAudioRequest
        {
            Text = text,
            Speed = ToRequestSpeed(options?.Speed ?? 1.0),
            VoiceType = Convert.ToInt32(options?.VoiceId ?? "1", CultureInfo.InvariantCulture),
        };

        var reqJson = JsonSerializer.Serialize(req, JsonGenContext.Default.TencentAudioRequest);
        using var request = AuthorizeTool.CreateAuthorizedRequest(new(_apiEndpoint), reqJson, _config!.SecretId, _config.AccessKey, _action, "2019-08-23");
        using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new KernelException("Tencent TTS failed.", new HttpRequestException(content));
        }

        var responseObj = JsonSerializer.Deserialize(content, JsonGenContext.Default.TencentAudioResponse)
            ?? throw new KernelException("The server returned an empty result.");
        return responseObj.Response is null
            ? throw new KernelException("The server returned an empty result.")
            : responseObj.Response.Error != null
            ? throw new KernelException($"{responseObj.Response.Error.Code}: {responseObj.Response.Error.Message}")
            : BinaryData.FromBytes(Convert.FromBase64String(responseObj.Response.Audio!), "audio/wav");
    }

    private static double ToRequestSpeed(double real)
    {
        if (real is > 2.5 or < 0.6)
        {
            return 0; // 无效入参
        }

        double arg;

        if (real is >= 0.6 and < 0.8)
        {
            arg = -2 + ((real - 0.6) / (0.8 - 0.6) * ((-1) - (-2)));
        }
        else if (real is >= 0.8 and < 1.0)
        {
            arg = -1 + ((real - 0.8) / (1.0 - 0.8) * (0 - (-1)));
        }
        else if (real is >= 1.0 and < 1.2)
        {
            arg = 0 + ((real - 1.0) / (1.2 - 1.0) * (1 - 0));
        }
        else if (real is >= 1.2 and < 1.5)
        {
            arg = 1 + ((real - 1.2) / (1.5 - 1.2) * (2 - 1));
        }
        else if (real is >= 1.5 and <= 2.5)
        {
            arg = 2 + ((real - 1.5) / (2.5 - 1.5) * (6 - 2));
        }
        else
        {
            return 0; // This should never be reached due to the initial check
        }

        // 保留2位小数
        return Math.Round(arg, 2);
    }
}
