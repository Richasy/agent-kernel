// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Audio;
using Richasy.AgentKernel.Connectors.OpenAI.Core;
using Richasy.AgentKernel.Connectors.OpenAI.Models;
using Richasy.AgentKernel.Models;
using RichasyKernel;

namespace Richasy.AgentKernel.Connectors.OpenAI;

/// <summary>
/// Represents an audio service that uses OpenAI.
/// </summary>
public sealed class OpenAIAudioService : IAudioService
{
    private OpenAIServiceConfig? _config;

    /// <inheritdoc/>
    public IAudioClient? Client { get; set; }

    /// <inheritdoc/>
    public AIServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig? config)
    {
        if (config is not OpenAIServiceConfig aiConfig)
        {
            throw new KernelException("Configuration is invalid");
        }

        if (_config != null && aiConfig.Equals(_config))
        {
            return;
        }

        _config = aiConfig;
        Client?.Dispose();
        Client = new OpenAIAudioClient(aiConfig);
    }

    /// <inheritdoc/>
    public IReadOnlyList<AudioModel> GetPredefinedModels() =>
    [
        new AudioModel
        {
            Id = "tts-1",
            DisplayName = "TTS",
            Voices = GetOpenAIAudioVoices(),
        },
        new AudioModel
        {
            Id = "tts-1-hd",
            DisplayName = "TTS HD",
            Voices = GetOpenAIAudioVoices(),
        },
    ];

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
