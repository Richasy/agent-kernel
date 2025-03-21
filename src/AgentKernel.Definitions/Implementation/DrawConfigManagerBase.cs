// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel;

/// <summary>
/// 绘图配置管理器.
/// </summary>
public abstract class DrawConfigManagerBase : IDrawConfigManager
{
    /// <inheritdoc/>
    public DrawClientConfiguration? Configuration { get; private set; }

    /// <inheritdoc/>
    public async Task<DrawClientConfigBase?> GetDrawConfigAsync(DrawProviderType provider)
    {
        await InitializeAsync().ConfigureAwait(false);
        return provider switch
        {
            DrawProviderType.Spark => Configuration?.Spark,
            DrawProviderType.Hunyuan => Configuration?.Hunyuan,
            DrawProviderType.Ernie => Configuration?.Ernie,
            DrawProviderType.OpenAI => Configuration?.OpenAI,
            DrawProviderType.AzureOpenAI => Configuration?.AzureOpenAI,
            DrawProviderType.XAI => Configuration?.XAI,
            DrawProviderType.ZhiPu => Configuration?.ZhiPu,
            _ => throw new NotImplementedException(),
        };
    }

    /// <inheritdoc/>
    public async Task SaveDrawConfigAsync(Dictionary<DrawProviderType, DrawClientConfigBase> configMap)
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
                case DrawProviderType.Spark:
                    Configuration.Spark = item.Value as SparkDrawConfig;
                    break;
                case DrawProviderType.Hunyuan:
                    Configuration.Hunyuan = item.Value as HunyuanDrawConfig;
                    break;
                case DrawProviderType.Ernie:
                    Configuration.Ernie = item.Value as ErnieDrawConfig;
                    break;
                case DrawProviderType.OpenAI:
                    Configuration.OpenAI = item.Value as OpenAIDrawConfig;
                    break;
                case DrawProviderType.AzureOpenAI:
                    Configuration.AzureOpenAI = item.Value as AzureOpenAIDrawConfig;
                    break;
                case DrawProviderType.XAI:
                    Configuration.XAI = item.Value as XAIDrawConfig;
                    break;
                case DrawProviderType.ZhiPu:
                    Configuration.ZhiPu = item.Value as ZhiPuDrawConfig;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        await OnSaveAsync(Configuration).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<AIServiceConfig?> GetServiceConfigAsync(DrawProviderType provider, DrawModel model)
    {
        var config = await GetDrawConfigAsync(provider).ConfigureAwait(false);
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
    /// <returns><see cref="DrawClientConfiguration"/>.</returns>
    protected abstract Task<DrawClientConfiguration> OnInitializeAsync();

    /// <summary>
    /// Save the configuration.
    /// </summary>
    protected abstract Task OnSaveAsync(DrawClientConfiguration configuration);

    /// <summary>
    /// 转换为配置.
    /// </summary>
    protected abstract AIServiceConfig? ConvertToConfig(DrawClientConfigBase? config);

    private async Task InitializeAsync()
    {
        if (Configuration is not null)
        {
            return;
        }

        Configuration = await OnInitializeAsync().ConfigureAwait(false);
    }
}
