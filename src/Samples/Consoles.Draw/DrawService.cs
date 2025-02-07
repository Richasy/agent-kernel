// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Hosting;
using Richasy.AgentKernel.Draw;
using Richasy.AgentKernel.Models;
using RichasyKernel;
using Spectre.Console;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using Richasy.AgentKernel;

namespace Consoles.Draw;

#pragma warning disable CA1001 // 具有可释放字段的类型应该是可释放的
internal sealed class DrawService(Kernel kernel, IDrawConfigManager configManager, IHostApplicationLifetime lifetime) : IHostedService
#pragma warning restore CA1001 // 具有可释放字段的类型应该是可释放的
{
    private readonly CancellationTokenSource _stopCts = new();
    private Task? _chatTask;
    private DrawModel? _model;

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

    private DrawProviderType AskProvider()
    {
        var providers = Enum.GetValues<DrawProviderType>();
        return AnsiConsole.Prompt(new SelectionPrompt<DrawProviderType>()
            .Title("Select a provider")
            .PageSize(20)
            .MoreChoicesText("More")
            .UseConverter(ProviderToName)
            .AddChoices(providers));
    }

    private DrawModel? AskModel(DrawProviderType providerType)
    {
        var provider = kernel.GetRequiredService<IDrawService>(providerType.ToString());
        var models = provider.GetPredefinedModels();
        return models?.Count > 1
            ? AnsiConsole.Prompt(new SelectionPrompt<DrawModel>()
            .Title("Select a model")
            .PageSize(20)
            .MoreChoicesText("More")
            .UseConverter(x => x.DisplayName ?? x.Id)
            .AddChoices(models))
            : models?.FirstOrDefault();
    }

    private static DrawSize? AskSize(DrawModel model)
    {
        var sizes = model.SupportSizes;
        return sizes?.Count > 1
            ? AnsiConsole.Prompt(new SelectionPrompt<DrawSize>()
            .Title("Select a size")
            .PageSize(20)
            .MoreChoicesText("More")
            .UseConverter(x => $"{x.Width}x{x.Height}")
            .AddChoices(sizes))
            : sizes?.FirstOrDefault();
    }

    private string ProviderToName(DrawProviderType provider)
    {
        return provider switch
        {
            DrawProviderType.OpenAI  => "OpenAI",
            DrawProviderType.AzureOpenAI => "Azure OpenAI",
            DrawProviderType.Ernie => "文心一言",
            DrawProviderType.Hunyuan => "混元",
            DrawProviderType.Spark => "星火",
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
            _model = AskModel(provider);
            var size = AskSize(_model!);
            var service = await DispatchServiceAsync(provider).ConfigureAwait(true);
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
                    ModelId = _model!.Id,
                    Width = size!.Value.Width,
                    Height = size!.Value.Height,
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

    private async Task<IDrawService> DispatchServiceAsync(DrawProviderType provider)
    {
        var config = await configManager.GetServiceConfigAsync(provider, _model!).ConfigureAwait(true);
        var service = kernel.GetRequiredService<IDrawService>(provider.ToString());
        service.Initialize(config!);
        return service;
    }
}
