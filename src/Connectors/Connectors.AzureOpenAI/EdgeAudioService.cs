// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Audio;
using Richasy.AgentKernel.Connectors.Azure.Core;
using Richasy.AgentKernel.Models;
using System.Text.Json;

namespace Richasy.AgentKernel.Connectors.Azure;

/// <summary>
/// Azure audio service configuration.
/// </summary>
public sealed class EdgeAudioService : IAudioService
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

        Client = new EdgeAudioClient();
    }

    /// <inheritdoc/>
    public IReadOnlyList<AudioModel> GetPredefinedModels()
    {
        if (_defaultModel == null)
        {
            var localJson = File.ReadAllText("EdgeVoiceList.json");
            var localVoices = JsonSerializer.Deserialize(localJson, JsonGenContext.Default.ListEdgeVoice);
            var voices = localVoices!.ConvertAll(x =>
            {
                var gender = x.Gender switch
                {
                    "Male" => VoiceGender.Male,
                    "Female" => VoiceGender.Female,
                    _ => VoiceGender.Neutral,
                };
                return new AudioVoice(
                    x.Name!,
                    x.FriendlyName!.Split('-').First().Replace("Microsoft ", string.Empty, StringComparison.InvariantCulture).Replace("Online ", string.Empty, StringComparison.InvariantCulture).Trim(),
                    gender,
                    x.Locale!)
                {
                    Codec = x.SuggestedCodec
                };
            });

            _defaultModel = new AudioModel
            {
                Id = "Edge",
                DisplayName = "Edge",
                Voices = voices
            };
        }

        return [_defaultModel];
    }
}
