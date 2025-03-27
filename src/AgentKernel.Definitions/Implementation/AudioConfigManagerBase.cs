// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel;

/// <summary>
/// 音频配置管理器.
/// </summary>
public abstract class AudioConfigManagerBase : IAudioConfigManager
{
    /// <inheritdoc/>
    public AudioClientConfiguration? Configuration { get; private set; }

    /// <inheritdoc/>
    public async Task<AudioClientConfigBase?> GetAudioConfigAsync(AudioProviderType provider)
    {
        await InitializeAsync().ConfigureAwait(false);
        return provider switch
        {
            AudioProviderType.Azure => Configuration?.AzureSpeech,
            AudioProviderType.AzureOpenAI => Configuration?.AzureOpenAI,
            AudioProviderType.OpenAI => Configuration?.OpenAI,
            AudioProviderType.Volcano => Configuration?.Volcano,
            AudioProviderType.Tencent => Configuration?.Tencent,
            _ => default,
        };
    }

    /// <inheritdoc/>
    public async Task SaveAudioConfigAsync(Dictionary<AudioProviderType, AudioClientConfigBase> configMap)
    {
        await InitializeAsync().ConfigureAwait(false);
        if (Configuration is null)
        {
            throw new InvalidOperationException("Configuration is not initialized.");
        }

        foreach (var item in configMap)
        {
            switch (item.Key)
            {
                case AudioProviderType.OpenAI:
                    Configuration.OpenAI = item.Value as OpenAIAudioConfig;
                    break;
                case AudioProviderType.AzureOpenAI:
                    Configuration.AzureOpenAI = item.Value as AzureOpenAIAudioConfig;
                    break;
                case AudioProviderType.Azure:
                    Configuration.AzureSpeech = item.Value as AzureAudioConfig;
                    break;
                case AudioProviderType.Volcano:
                    Configuration.Volcano = item.Value as VolcanoAudioConfig;
                    break;
                case AudioProviderType.Tencent:
                    Configuration.Tencent = item.Value as TencentAudioConfig;
                    break;
                default:
                    break;
            }
        }

        await OnSaveAsync(Configuration).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<AIServiceConfig?> GetServiceConfigAsync(AudioProviderType provider, AudioModel model)
    {
        var config = await GetAudioConfigAsync(provider).ConfigureAwait(false);
        var aiConfig = ConvertToConfig(config);
        if (aiConfig is not null)
        {
            aiConfig.Model = model.Id;
        }

        return aiConfig;
    }

    /// <summary>
    /// Initialize the configuration.
    /// </summary>
    /// <returns><see cref="AudioClientConfiguration"/>.</returns>
    protected abstract Task<AudioClientConfiguration> OnInitializeAsync();

    /// <summary>
    /// Save the configuration.
    /// </summary>
    protected abstract Task OnSaveAsync(AudioClientConfiguration configuration);

    /// <summary>
    /// 转换为配置.
    /// </summary>
    protected abstract AIServiceConfig? ConvertToConfig(AudioClientConfigBase? config);

    private async Task InitializeAsync()
    {
        if (Configuration is not null)
        {
            return;
        }

        Configuration = await OnInitializeAsync().ConfigureAwait(false);
    }
}
