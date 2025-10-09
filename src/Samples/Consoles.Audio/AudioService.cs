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
internal sealed class AudioService(Kernel kernel, IAudioConfigManager configManager, IHostApplicationLifetime lifetime) : IHostedService
#pragma warning restore CA1001 // 具有可释放字段的类型应该是可释放的
{
    private readonly CancellationTokenSource _stopCts = new();
    private Task? _chatTask;
    private AudioModel? _model;

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
            AudioProviderType.Volcano,
            AudioProviderType.Tencent,
        ];
        return AnsiConsole.Prompt(new SelectionPrompt<AudioProviderType>()
            .Title("Select a provider")
            .PageSize(20)
            .MoreChoicesText("More")
            .UseConverter(ProviderToName)
            .AddChoices(providers));
    }

    private async Task<AudioModel?> AskModelAsync(AudioProviderType providerType)
    {
        var provider = kernel.GetRequiredService<IAudioService>(providerType.ToString());
        var models = provider.GetPredefinedModels().ToList();
        var config = await configManager.GetAudioConfigAsync(providerType).ConfigureAwait(true);
        if (config?.IsCustomModelNotEmpty() == true)
        {
            models.AddRange(config.CustomModels!);
        }

        return models?.Count > 1
            ? AnsiConsole.Prompt(new SelectionPrompt<AudioModel>()
            .Title("Select a model")
            .PageSize(20)
            .MoreChoicesText("More")
            .UseConverter(x => x.Name ?? x.Id)
            .AddChoices(models))
            : models?.FirstOrDefault();
    }

#pragma warning disable CA1822 // Mark members as static
    private AudioVoice? AskVoice(AudioModel model)
#pragma warning restore CA1822 // Mark members as static
    {
        var voices = model.Voices.Where(p => p.Languages.Contains("en") || p.Languages.Contains("en-US"));
        return voices?.Count() > 1
            ? AnsiConsole.Prompt(new SelectionPrompt<AudioVoice>()
            .Title("Select a voice")
            .PageSize(20)
            .MoreChoicesText("More")
            .UseConverter(x => x.Id + (x.Gender == VoiceGender.Male ? "♂️" : "♀️"))
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
            AudioProviderType.Volcano => "火山",
            AudioProviderType.Tencent => "腾讯",
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
            _model = await AskModelAsync(provider).ConfigureAwait(true);
            var service = await DispatchServiceAsync(provider).ConfigureAwait(true);
            var voice = AskVoice(_model!);
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
                    LanguageCode = "zh-CN",
                    VoiceId = voice!.Id,
                };

                options.ModelId = _model!.Id;
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

    private async Task<IAudioService> DispatchServiceAsync(AudioProviderType provider)
    {
        var config = await configManager.GetServiceConfigAsync(provider, _model!).ConfigureAwait(true);
        var service = kernel.GetRequiredService<IAudioService>(provider.ToString());
        service.Initialize(config);
        return service;
    }
}
