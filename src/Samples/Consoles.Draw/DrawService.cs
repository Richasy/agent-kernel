// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Hosting;
using Richasy.AgentKernel.Draw;
using Richasy.AgentKernel.Models;
using RichasyKernel;
using Spectre.Console;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using Richasy.AgentKernel.Connectors.Baidu.Models;
using Richasy.AgentKernel.Connectors.Tencent.Models;

namespace Consoles.Draw;

#pragma warning disable CA1001 // 具有可释放字段的类型应该是可释放的
internal sealed class DrawService(Kernel kernel, DrawConfiguration config, IHostApplicationLifetime lifetime) : IHostedService
#pragma warning restore CA1001 // 具有可释放字段的类型应该是可释放的
{
    private readonly CancellationTokenSource _stopCts = new();
    private Task? _chatTask;
    private string? _model;
    private int _width;
    private int _height;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _chatTask = Task.Run(() => RunDrawAsync(_stopCts.Token), cancellationToken);
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
            ProviderType.AzureOpenAI => "Azure OpenAI",
            ProviderType.Ernie => "文心一言",
            ProviderType.Hunyuan => "混元",
            ProviderType.Spark => "星火",
            _ => throw new NotSupportedException(),
        };
    }

    [UnconditionalSuppressMessage("AOT", "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.", Justification = "<Pending>")]
    private async Task RunDrawAsync(CancellationToken cancellationToken)
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

                var options = new DrawOptions()
                {
                    ModelId = _model,
                    Width = _width,
                    Height = _height,
                };

                var result = await service.Client!.DrawAsync(input, options, cancellationToken).ConfigureAwait(true);
                await ReadResultAsync(result).ConfigureAwait(true);
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex);
            throw;
        }
    }

    private async Task ReadResultAsync(BinaryData result)
    {
        _ = this;
        var extension = result.MediaType switch
        {
            "image/png" => "png",
            "image/jpeg" => "jpg",
            _ => "png",
        };

        var tempDrawPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"temp.{extension}");
        await File.WriteAllBytesAsync(tempDrawPath, result.ToArray()).ConfigureAwait(true);
        Process.Start(new ProcessStartInfo(tempDrawPath) { UseShellExecute = true });
    }

    private IDrawService DispatchService(ProviderType provider)
    {
        var service = kernel.GetRequiredService<IDrawService>(provider.ToString());
        switch (provider)
        {
            case ProviderType.AzureOpenAI:
                _model = config.AzureOpenAI!.Model;
                DispatchSize(config.AzureOpenAI!.Size);
                break;
            case ProviderType.Ernie:
                _model = config.Ernie!.Model;
                DispatchSize(config.Ernie!.Size);
                break;
            case ProviderType.Hunyuan:
                DispatchSize(config.Hunyuan!.Size);
                break;
            case ProviderType.Spark:
                _model = config.Spark!.Model;
                DispatchSize(config.Spark!.Size);
                break;
            default:
                break;
        }

        var serviceConfig = provider switch
        {
            ProviderType.AzureOpenAI => config.AzureOpenAI.ToAIServiceConfig(),
            ProviderType.Ernie => config.Ernie.ToAIServiceConfig<ErnieServiceConfig>(),
            ProviderType.Hunyuan => config.Hunyuan.ToAIServiceConfig<HunyuanDrawServiceConfig>(),
            ProviderType.Spark => config.Spark.ToAIServiceConfig(),
            _ => throw new NotSupportedException(),
        };

        service.Initialize(serviceConfig!);
        return service;
    }

    private void DispatchSize(string? size)
    {
        if (string.IsNullOrWhiteSpace(size))
        {
            _width = 1024;
            _height = 1024;
            return;
        }

        var parts = size.Split('x');
        if (parts.Length == 2)
        {
            _width = int.Parse(parts[0]);
            _height = int.Parse(parts[1]);
        }
    }
}
