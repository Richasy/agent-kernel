// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Audio;
using Richasy.AgentKernel.Connectors.Windows.Core;
using Richasy.AgentKernel.Models;
using Wms = Windows.Media.SpeechSynthesis;

namespace Richasy.AgentKernel.Connectors.Windows;

/// <summary>
/// Windows audio service.
/// </summary>
public sealed class WindowsAudioService : IAudioService
{
    private AudioModel? _defaultModel;

    /// <inheritdoc/>
    public IAudioClient? Client { get; set; }

    /// <inheritdoc/>
    public AIServiceConfig? Config => default;

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig? config)
    {
        if (Client is not null)
        {
            return;
        }

        Client = new WindowsAudioClient();
    }

    /// <inheritdoc/>
    public IReadOnlyList<AudioModel> GetPredefinedModels()
    {
        if (_defaultModel == null)
        {
            var allVoices = Wms.SpeechSynthesizer.AllVoices;
            var voices = allVoices.Select(x =>
            {
                var gender = x.Gender switch
                {
                    Wms.VoiceGender.Male => VoiceGender.Male,
                    Wms.VoiceGender.Female => VoiceGender.Female,
                    _ => VoiceGender.Neutral,
                };

                return new AudioVoice(x.Id, x.DisplayName, gender, x.Language);
            }).ToList();

            _defaultModel = new AudioModel { Id = "Local", Name = "Windows", Voices = voices };
        }

        return [_defaultModel];
    }
}
