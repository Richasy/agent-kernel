// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel;

/// <summary>
/// 翻译配置管理器.
/// </summary>
public abstract class TranslateConfigManagerBase : ITranslateConfigManager
{
    /// <inheritdoc/>
    public TranslateClientConfiguration? Configuration { get; private set; }

    /// <inheritdoc/>
    public async Task<TranslateClientConfigBase?> GetTranslateConfigAsync(TranslateProviderType provider)
    {
        await InitializeAsync().ConfigureAwait(false);
        return provider switch
        {
            TranslateProviderType.Azure => Configuration?.Azure,
            TranslateProviderType.Baidu => Configuration?.Baidu,
            TranslateProviderType.Tencent => Configuration?.Tencent,
            TranslateProviderType.Ali => Configuration?.Ali,
            TranslateProviderType.Youdao => Configuration?.Youdao,
            TranslateProviderType.Volcano => Configuration?.Volcano,
            _ => default,
        };
    }

    /// <inheritdoc/>
    public async Task SaveTranslateConfigAsync(Dictionary<TranslateProviderType, TranslateClientConfigBase> configMap)
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
                case TranslateProviderType.Azure:
                    Configuration.Azure = item.Value as AzureTranslateConfig;
                    break;
                case TranslateProviderType.Baidu:
                    Configuration.Baidu = item.Value as BaiduTranslateConfig;
                    break;
                case TranslateProviderType.Tencent:
                    Configuration.Tencent = item.Value as TencentTranslateConfig;
                    break;
                case TranslateProviderType.Ali:
                    Configuration.Ali = item.Value as AliTranslateConfig;
                    break;
                case TranslateProviderType.Youdao:
                    Configuration.Youdao = item.Value as YoudaoTranslateConfig;
                    break;
                case TranslateProviderType.Volcano:
                    Configuration.Volcano = item.Value as VolcanoTranslateConfig;
                    break;
                default:
                    break;
            }
        }

        await OnSaveAsync(Configuration).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<TranslateServiceConfig?> GetServiceConfigAsync(TranslateProviderType provider)
    {
        var config = await GetTranslateConfigAsync(provider).ConfigureAwait(false);
        return ConvertToConfig(config);
    }

    /// <summary>
    /// Initialize the configuration.
    /// </summary>
    /// <returns><see cref="TranslateClientConfiguration"/>.</returns>
    protected abstract Task<TranslateClientConfiguration> OnInitializeAsync();

    /// <summary>
    /// Save the configuration.
    /// </summary>
    protected abstract Task OnSaveAsync(TranslateClientConfiguration configuration);

    /// <summary>
    /// 转换为配置.
    /// </summary>
    protected abstract TranslateServiceConfig? ConvertToConfig(TranslateClientConfigBase? config);

    private async Task InitializeAsync()
    {
        if (Configuration is not null)
        {
            return;
        }

        Configuration = await OnInitializeAsync().ConfigureAwait(false);
    }
}
