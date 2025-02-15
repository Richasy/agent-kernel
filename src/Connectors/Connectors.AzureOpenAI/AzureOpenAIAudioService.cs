// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Audio;
using Richasy.AgentKernel.Connectors.Azure.Core;
using Richasy.AgentKernel.Connectors.Azure.Models;
using Richasy.AgentKernel.Models;
using RichasyKernel;

namespace Richasy.AgentKernel.Connectors.Azure;

/// <summary>
/// Represents an audio service that uses Azure.
/// </summary>
public sealed class AzureOpenAIAudioService : IAudioService
{
    private AzureOpenAIServiceConfig? _config;

    /// <inheritdoc/>
    public IAudioClient? Client { get; set; }

    /// <inheritdoc/>
    public AIServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig? config)
    {
        if (config is not AzureOpenAIServiceConfig aiConfig)
        {
            throw new KernelException("Configuration is invalid");
        }

        if (_config != null && aiConfig.Equals(_config))
        {
            return;
        }

        _config = aiConfig;
        Client?.Dispose();
        Client = new AzureOpenAIAudioClient(aiConfig);
    }

    /// <inheritdoc/>
    public IReadOnlyList<AudioModel> GetPredefinedModels()
    {
        return
        [
            new() {
                Id = "tts",
                Name = "TTS",
                Voices = GetOpenAIAudioVoices(),
            },
            new() {
                Id = "tts-hd",
                Name = "TTS HD",
                Voices = GetOpenAIAudioVoices(),
            },
        ];
    }

    private static string[] GetOpenAIAudioLanguages()
    {
        return
        [
            "af", "ar", "hy", "az", "be", "bs", "bg", "ca", "zh",
            "hr", "cs", "da", "nl", "en", "et", "fi", "fr", "gl",
            "de", "el", "he", "hi", "hu", "is", "id", "it"
        ];
    }

    private static List<AudioVoice> GetOpenAIAudioVoices()
    {
        return
        [
            new("alloy", "Alloy", VoiceGender.Male, GetOpenAIAudioLanguages()),
            new("echo", "Echo", VoiceGender.Male, GetOpenAIAudioLanguages()),
            new("fable", "Fable", VoiceGender.Male, GetOpenAIAudioLanguages()),
            new("onyx", "Onyx", VoiceGender.Male, GetOpenAIAudioLanguages()),
            new("nova", "Nova", VoiceGender.Female, GetOpenAIAudioLanguages()),
            new("shimmer", "Shimmer", VoiceGender.Female, GetOpenAIAudioLanguages()),
        ];
    }
}
