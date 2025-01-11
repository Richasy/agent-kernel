// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Hosting;
using Richasy.AgentKernel.Audio;
using Richasy.AgentKernel.Models;
using RichasyKernel;
using Spectre.Console;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Consoles.Audio;

#pragma warning disable CA1001 // 具有可释放字段的类型应该是可释放的
internal sealed class AudioService(Kernel kernel, AudioConfiguration config, IHostApplicationLifetime lifetime) : IHostedService
#pragma warning restore CA1001 // 具有可释放字段的类型应该是可释放的
{
    private readonly CancellationTokenSource _stopCts = new();
    private Task? _chatTask;
    private string? _languageCode;
    private string? _voiceId;
    private string? _model;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _chatTask = Task.Run(() => RunAudioAsync(_stopCts.Token), cancellationToken);
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
            ProviderType.Edge => "Edge",
            ProviderType.AzureOpenAI => "Azure OpenAI",
            _ => throw new NotSupportedException(),
        };
    }

    [UnconditionalSuppressMessage("AOT", "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.", Justification = "<Pending>")]
    private async Task RunAudioAsync(CancellationToken cancellationToken)
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

                var options = new AudioOptions()
                {
                    Speed = 1.0,
                    LanguageCode = _languageCode,
                    VoiceId = _voiceId,
                };

                var result = await service.Client!.TextToSpeechAsync(input, options, cancellationToken).ConfigureAwait(true);
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
        var tempAudioPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp.wav");
        await File.WriteAllBytesAsync(tempAudioPath, result.ToArray()).ConfigureAwait(true);
        Process.Start(new ProcessStartInfo(tempAudioPath) { UseShellExecute = true });
    }

    private IAudioService DispatchService(ProviderType provider)
    {
        var service = kernel.GetRequiredService<IAudioService>(provider.ToString());
        switch (provider)
        {
            case ProviderType.Azure:
                _voiceId = config.Azure!.Voice;
                _languageCode = config.Azure.Language;
                break;
            case ProviderType.Edge:
                _voiceId = config.Edge!.Voice;
                _languageCode = config.Edge.Language;
                break;
            case ProviderType.AzureOpenAI:
                _voiceId = config.AzureOpenAI!.Voice;
                _languageCode = config.AzureOpenAI.Language;
                _model = config.AzureOpenAI.Model;
                break;
            default:
                break;
        }

        var serviceConfig = provider switch
        {
            ProviderType.Azure => config.Azure.ToAIServiceConfig(),
            ProviderType.AzureOpenAI => config.AzureOpenAI.ToAIServiceConfig(),
            ProviderType.Edge => null,
            _ => throw new NotSupportedException(),
        };

        service.Initialize(serviceConfig);
        return service;
    }
}
