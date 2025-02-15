// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Audio;
using Richasy.AgentKernel.Connectors.Azure.Core;
using Richasy.AgentKernel.Connectors.Azure.Models;
using Richasy.AgentKernel.Models;
using RichasyKernel;
using System.Text.Json;

namespace Richasy.AgentKernel.Connectors.Azure;

/// <summary>
/// Represents a service that can generate audio using Azure services.
/// </summary>
public sealed class AzureAudioService : IAudioService
{
    private AzureAudioServiceConfig? _config;
    private AudioModel? _defaultModel;

    /// <inheritdoc/>
    public IAudioClient? Client { get; set; }

    /// <inheritdoc/>
    public AIServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig? config)
    {
        if (config is not AzureAudioServiceConfig azureConfig)
        {
            throw new KernelException("Configuration is invalid");
        }

        if (_config != null && azureConfig.Equals(_config))
        {
            return;
        }

        _config = azureConfig;
        Client?.Dispose();
        Client = new AzureAudioClient(azureConfig);
    }

    /// <inheritdoc/>
    public IReadOnlyList<AudioModel> GetPredefinedModels()
    {
        if (_defaultModel == null)
        {
            var localJson = VoiceConstants.AzureVoices;
            var localVoices = JsonSerializer.Deserialize(localJson, JsonGenContext.Default.ListAzureVoice);
            var voices = localVoices!.ConvertAll(x =>
            {
                var gender = x.Gender switch
                {
                    "Male" => VoiceGender.Male,
                    "Female" => VoiceGender.Female,
                    _ => VoiceGender.Neutral,
                };
                return new AudioVoice(
                    x.ShortName!,
                    x.LocaleName!,
                    gender,
                    x.Locale!);
            });

            _defaultModel = new AudioModel
            {
                Id = "Azure",
                Name = "Azure",
                Voices = voices
            };
        }

        return [_defaultModel];
    }
}
