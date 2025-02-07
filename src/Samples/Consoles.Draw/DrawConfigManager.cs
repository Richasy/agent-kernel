// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel;
using Richasy.AgentKernel.Models;
using System.Text.Json;

namespace Consoles.Draw;

/// <summary>
/// 绘图配置管理器.
/// </summary>
internal sealed class DrawConfigManager : DrawConfigManagerBase
{
    protected override async Task<DrawClientConfiguration> OnInitializeAsync()
    {
        var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "env.json");
        if (!File.Exists(configPath))
        {
            throw new FileNotFoundException("Config file not found.");
        }

        var configContent = await File.ReadAllTextAsync(configPath).ConfigureAwait(false);
        return JsonSerializer.Deserialize(configContent, JsonGenerationContext.Default.DrawClientConfiguration)!;
    }

    protected override Task OnSaveAsync(DrawClientConfiguration configuration) => Task.CompletedTask;

    protected override AIServiceConfig? ConvertToConfig(DrawClientConfigBase? config)
    {
        return config switch
        {
            OpenAIDrawConfig openAIConfig => openAIConfig.ToAIServiceConfig(),
            AzureOpenAIDrawConfig azureOaiConfig => azureOaiConfig.ToAIServiceConfig(),
            ErnieDrawConfig ernieConfig => ernieConfig.ToAIServiceConfig(),
            HunyuanDrawConfig hunyuanConfig => hunyuanConfig.ToAIServiceConfig(),
            SparkDrawConfig sparkConfig => sparkConfig.ToAIServiceConfig(),
            _ => null,
        };
    }
}
