// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Hosting;
using Richasy.AgentKernel.Connectors.Ali.Models;
using Richasy.AgentKernel.Connectors.Baidu.Models;
using Richasy.AgentKernel.Connectors.Google.Models;
using Richasy.AgentKernel.Connectors.Tencent.Models;
using Richasy.AgentKernel.Connectors.Volcano.Models;
using Richasy.AgentKernel.Connectors.Youdao.Models;
using Richasy.AgentKernel.Models;
using Richasy.AgentKernel.Translation;
using RichasyKernel;
using Spectre.Console;
using System.Diagnostics.CodeAnalysis;

namespace Consoles.Translation;

#pragma warning disable CA1001 // 具有可释放字段的类型应该是可释放的
internal sealed class TranslationService(Kernel kernel, TranslationConfiguration config, IHostApplicationLifetime lifetime) : IHostedService
#pragma warning restore CA1001 // 具有可释放字段的类型应该是可释放的
{
    private readonly CancellationTokenSource _stopCts = new();
    private Task? _chatTask;
    private string? _sourceLanguage;
    private string? _targetLanguage;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _chatTask = Task.Run(() => RunTranslateAsync(_stopCts.Token), cancellationToken);
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_chatTask != null)
        {
            await _stopCts.CancelAsync().ConfigureAwait(true);
        }
    }

    private ProviderType AskProvider()
    {
        var providers = Enum.GetValues<ProviderType>();
        return AnsiConsole.Prompt(new SelectionPrompt<ProviderType>()
            .Title("Select a provider")
            .PageSize(20)
            .MoreChoicesText("More")
            .UseConverter(ProviderToName)
            .AddChoices(providers));
    }

    private string ProviderToName(ProviderType provider)
    {
        return provider switch
        {
            ProviderType.Azure => "Azure",
            ProviderType.Ali => "阿里云",
            ProviderType.Baidu => "百度",
            ProviderType.Tencent => "腾讯",
            ProviderType.Volcano => "火山",
            ProviderType.Youdao => "有道",
            ProviderType.Google => "Google",
            _ => throw new NotSupportedException(),
        };
    }

    [UnconditionalSuppressMessage("AOT", "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.", Justification = "<Pending>")]
    private async Task RunTranslateAsync(CancellationToken cancellationToken)
    {
        try
        {
            AnsiConsole.Clear();
            var provider = AskProvider();
            var service = DispatchService(provider);
            while (!cancellationToken.IsCancellationRequested)
            {
                var input = AnsiConsole.Prompt(
                    new TextPrompt<string>("[grey]>>>[/] ")
                    .Validate(x => !string.IsNullOrWhiteSpace(x) ? ValidationResult.Success() : ValidationResult.Error("The text cannot be empty.")));
                if (input == "bye")
                {
                    lifetime.StopApplication();
                    break;
                }
                else if (input == "clear")
                {
                    AnsiConsole.Clear();
                    continue;
                }

                var options = new TranslateOptions()
                {
                    SourceLanguage = _sourceLanguage,
                    TargetLanguage = _targetLanguage,
                };
                var result = await service.TranslateTextAsync(input, options, cancellationToken).ConfigureAwait(true);
                PrintResult(result);
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex);
            throw;
        }
    }

    private void PrintResult(TranslateCompletion result)
    {
        _ = this;
        var text = result.Result;
        if (string.IsNullOrEmpty(text))
        {
            return;
        }

        var panel = new Panel(text.EscapeMarkup())
        {
            Border = BoxBorder.Rounded,
            Expand = true,
            Padding = new Padding(2, 2, 2, 2),
        };

        if (!string.IsNullOrEmpty(result.TargetLanguage))
        {
            panel.Header = new PanelHeader(result.TargetLanguage);
        }

        AnsiConsole.Write(panel);
    }

    private ITextTranslationService DispatchService(ProviderType provider)
    {
        var service = kernel.GetRequiredService<ITextTranslationService>(provider.ToString());
        var originConfig = provider switch
        {
            ProviderType.Azure => config.Azure,
            ProviderType.Ali => config.Ali,
            ProviderType.Baidu => config.Baidu,
            ProviderType.Tencent => config.Tencent,
            ProviderType.Volcano => config.Volcano,
            ProviderType.Youdao => config.Youdao,
            ProviderType.Google => config.Google,
            _ => throw new NotSupportedException(),
        };

        var serviceConfig = provider switch
        {
            ProviderType.Azure => config.Azure.ToTranslationServiceConfig(),
            ProviderType.Ali => config.Ali.ToTranslationServiceConfig<AliTranslationServiceConfig>(),
            ProviderType.Baidu => config.Baidu.ToTranslationServiceConfig<BaiduTranslationServiceConfig>(),
            ProviderType.Tencent => config.Tencent.ToTranslationServiceConfig<TencentTranslationServiceConfig>(),
            ProviderType.Volcano => config.Volcano.ToTranslationServiceConfig<VolcanoTranslationServiceConfig>(),
            ProviderType.Youdao => config.Youdao.ToTranslationServiceConfig<YoudaoTranslationServiceConfig>(),
            ProviderType.Google => new GoogleTranslationServiceConfig(string.Empty),
            _ => throw new NotSupportedException(),
        };

        _sourceLanguage = originConfig!.SourceLanguage;
        _targetLanguage = originConfig!.TargetLanguage;
        service.Initialize(serviceConfig!);
        return service;
    }
}
