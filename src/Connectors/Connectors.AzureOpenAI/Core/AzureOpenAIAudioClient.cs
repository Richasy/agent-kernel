// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Azure.AI.OpenAI;
using OpenAI.Audio;
using Richasy.AgentKernel.Audio;
using Richasy.AgentKernel.Connectors.Azure.Models;
using Richasy.AgentKernel.Models;
using System.ClientModel;

namespace Richasy.AgentKernel.Connectors.Azure.Core;

/// <summary>
/// Represents a client that can generate audio using Azure services.
/// </summary>
public sealed class AzureOpenAIAudioClient : IAudioClient
{
    private readonly AzureOpenAIServiceConfig _config;
    private readonly AzureOpenAIClient _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="AzureOpenAIAudioClient"/> class.
    /// </summary>
    public AzureOpenAIAudioClient(AzureOpenAIServiceConfig config)
    {
        _config = config;
        Metadata = new("azure_openai", config.Model);
        _client = new AzureOpenAIClient(_config.Endpoint, new ApiKeyCredential(_config.AccessKey));
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
        var model = options?.ModelId ?? _config.Model;
        var audioClient = _client.GetAudioClient(model);
        var voice = new GeneratedSpeechVoice(options!.VoiceId);
        var opt = new SpeechGenerationOptions
        {
            ResponseFormat = GeneratedSpeechFormat.Wav,
            SpeedRatio = (float?)options.Speed,
        };

        var result = await audioClient.GenerateSpeechAsync(text, voice, opt, cancellationToken).ConfigureAwait(false);
        return result.Value;
    }
}
