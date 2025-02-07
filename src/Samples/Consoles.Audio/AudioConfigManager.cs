// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;
using Richasy.AgentKernel;
using System.Text.Json;

namespace Consoles.Audio;

internal sealed class AudioConfigManager : AudioConfigManagerBase
{
    protected override async Task<AudioClientConfiguration> OnInitializeAsync()
    {
        var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "env.json");
        if (!File.Exists(configPath))
        {
            throw new FileNotFoundException("Config file not found.");
        }

        var configContent = await File.ReadAllTextAsync(configPath).ConfigureAwait(false);
        return JsonSerializer.Deserialize(configContent, JsonGenerationContext.Default.AudioClientConfiguration)!;
    }

    protected override Task OnSaveAsync(AudioClientConfiguration configuration) => Task.CompletedTask;

    protected override AIServiceConfig? ConvertToConfig(AudioClientConfigBase? config)
    {
        return config switch
        {
            OpenAIAudioConfig openAIConfig => openAIConfig.ToAIServiceConfig(),
            AzureOpenAIAudioConfig azureOaiConfig => azureOaiConfig.ToAIServiceConfig(),
            AzureAudioConfig azureConfig => azureConfig.ToAIServiceConfig(),
            _ => null,
        };
    }
}
