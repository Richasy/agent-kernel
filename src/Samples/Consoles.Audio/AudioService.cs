// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Hosting;
using Richasy.AgentKernel;
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

    private AudioProviderType AskProvider()
    {
        List<AudioProviderType> providers = [
            AudioProviderType.Azure,
            AudioProviderType.Edge,
            AudioProviderType.AzureOpenAI,
        ];
        return AnsiConsole.Prompt(new SelectionPrompt<AudioProviderType>()
            .Title("Select a provider")
            .PageSize(20)
            .MoreChoicesText("More")
            .UseConverter(ProviderToName)
            .AddChoices(providers));
    }

    private AudioModel? AskModel(AudioProviderType providerType)
    {
        var provider = kernel.GetRequiredService<IAudioService>(providerType.ToString());
        var models = provider.GetPredefinedModels();
        return models?.Count > 1
            ? AnsiConsole.Prompt(new SelectionPrompt<AudioModel>()
            .Title("Select a model")
            .PageSize(20)
            .MoreChoicesText("More")
            .UseConverter(x => x.DisplayName ?? x.Id)
            .AddChoices(models))
            : models?.FirstOrDefault();
    }

    private AudioVoice? AskVoice(AudioModel model)
    {
        var voices = model.Voices.Where(p=>p.Languages.Contains(_languageCode ?? "zh-CN"));
        return voices?.Count() > 1
            ? AnsiConsole.Prompt(new SelectionPrompt<AudioVoice>()
            .Title("Select a voice")
            .PageSize(20)
            .MoreChoicesText("More")
            .UseConverter(x => x.DisplayName + (x.Gender == VoiceGender.Male ? "♂️" : "♀️"))
            .AddChoices(voices))
            : voices?.FirstOrDefault();
    }

    private string ProviderToName(AudioProviderType provider)
    {
        return provider switch
        {
            AudioProviderType.Azure => "Azure",
            AudioProviderType.Edge => "Edge",
            AudioProviderType.AzureOpenAI => "Azure OpenAI",
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
            var model = AskModel(provider);
            var service = DispatchService(provider);
            var voice = AskVoice(model!);
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
                    VoiceId = voice!.Id,
                };

                if (!string.IsNullOrWhiteSpace(_model))
                {
                    options.ModelId = _model;
                }

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
        var tempAudioPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp2.wav");
        await File.WriteAllBytesAsync(tempAudioPath, result.ToArray()).ConfigureAwait(true);
        Process.Start(new ProcessStartInfo(tempAudioPath) { UseShellExecute = true });
    }

    private IAudioService DispatchService(AudioProviderType provider)
    {
        var service = kernel.GetRequiredService<IAudioService>(provider.ToString());
        switch (provider)
        {
            case AudioProviderType.Azure:
                _languageCode = config.Azure!.Language;
                break;
            case AudioProviderType.Edge:
                _languageCode = config.Edge!.Language;
                break;
            case AudioProviderType.AzureOpenAI:
                _languageCode = config.AzureOpenAI!.Language;
                _model = config.AzureOpenAI.Model;
                break;
            default:
                break;
        }

        var serviceConfig = provider switch
        {
            AudioProviderType.Azure => config.Azure.ToAIServiceConfig(),
            AudioProviderType.AzureOpenAI => config.AzureOpenAI.ToAIServiceConfig(),
            AudioProviderType.Edge => null,
            _ => throw new NotSupportedException(),
        };

        service.Initialize(serviceConfig);
        return service;
    }
}
