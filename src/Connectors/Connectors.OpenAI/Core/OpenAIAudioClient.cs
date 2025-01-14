// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Core.OpenAI;
using Richasy.AgentKernel.Core.OpenAI.Audio;
using Richasy.AgentKernel.Audio;
using Richasy.AgentKernel.Connectors.OpenAI.Models;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.OpenAI.Core;

/// <summary>
/// Audio client for OpenAI service.
/// </summary>
public sealed class OpenAIAudioClient : IAudioClient
{
    private readonly OpenAIServiceConfig _config;
    private readonly AudioClient _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenAIAudioClient"/> class.
    /// </summary>
    public OpenAIAudioClient(OpenAIServiceConfig config)
    {
        _config = config;
        Metadata = new("openai", config.Model);
        var options = new OpenAIClientOptions();
        if (_config.Endpoint != null)
        {
            options.Endpoint = _config.Endpoint;
        }

        _client = new AudioClient(config.Model!, new(config.AccessKey), options);
    }

    /// <inheritdoc/>
    public AudioClientMetadata Metadata { get; }

    /// <inheritdoc/>
    public void Dispose()
    {
    }

    /// <inheritdoc/>
    public async Task<BinaryData> TextToSpeechAsync(string text, AudioOptions? options, CancellationToken cancellationToken = default)
    {
        var voice = new GeneratedSpeechVoice(options!.VoiceId);
        var opt = new SpeechGenerationOptions
        {
            ResponseFormat = GeneratedSpeechFormat.Wav,
            SpeedRatio = (float?)options.Speed,
        };

        var result = await _client.GenerateSpeechAsync(text, voice, opt, cancellationToken).ConfigureAwait(false);
        return result.Value;
    }
}
