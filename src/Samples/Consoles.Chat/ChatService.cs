// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

#define USE_SYSTEM_PROMPT

using Microsoft.Extensions.AI;
using Microsoft.Extensions.Hosting;
using Richasy.AgentKernel;
using Richasy.AgentKernel.ChatCompletion;
using Spectre.Console;

namespace Consoles.Chat;

#pragma warning disable CA1001 // 具有可释放字段的类型应该是可释放的
#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks
internal sealed class ChatService(Kernel kernel, ChatConfiguration config, IHostApplicationLifetime lifetime) : IHostedService
{
    private readonly CancellationTokenSource _stopCts = new();
    private Task? _chatTask;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _chatTask = Task.Run(() => RunChatAsync(_stopCts.Token), cancellationToken);
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_chatTask != null)
        {
            await _stopCts.CancelAsync();
            await Task.WhenAny(_chatTask, Task.Delay(Timeout.Infinite, cancellationToken));
        }
    }

    private ProviderType AskProvider()
    {
        var providers = Enum.GetValues<ProviderType>();
        var provider = AnsiConsole.Prompt(new SelectionPrompt<ProviderType>()
            .Title("Select a provider")
            .PageSize(10)
            .MoreChoicesText("More")
            .UseConverter(ProviderToName)
            .AddChoices(providers));
        return provider;
    }

    private string ProviderToName(ProviderType provider)
    {
        return provider switch
        {
            ProviderType.AzureOpenAI => "Azure OpenAI",
            _ => throw new NotSupportedException(),
        };
    }

    private async Task RunChatAsync(CancellationToken cancellationToken)
    {
        var provider = AskProvider();
        var service = DispatchService(provider);
        AnsiConsole.Clear();
        var messages = new List<ChatMessage>();
#if USE_SYSTEM_PROMPT
        var sysPrompt = AnsiConsole.Prompt(
            new TextPrompt<string>("System prompt [green](Optional)[/]: ")
            .AllowEmpty());

        if (!string.IsNullOrEmpty(sysPrompt))
        {
            messages.Add(new ChatMessage(ChatRole.System, sysPrompt));
        }
#endif

        while (!cancellationToken.IsCancellationRequested)
        {
            AnsiConsole.WriteLine();

            var userPrompt = AnsiConsole.Prompt(
                new TextPrompt<string>("[grey]>>>[/] ")
                .Validate(x => !string.IsNullOrWhiteSpace(x) ? ValidationResult.Success() : ValidationResult.Error("The prompt cannot be empty.")));

            if (userPrompt == "bye")
            {
                lifetime.StopApplication();
                break;
            }

            messages.Add(new ChatMessage(ChatRole.User, userPrompt));
            var response = await service.Client!.CompleteAsync(messages, cancellationToken: cancellationToken);
            var responseMessage = response.Message.Text;
            messages.Add(new ChatMessage(ChatRole.Assistant, responseMessage));
            PrintAssistantMessage(response.Message);
        }
    }

    private IChatCompletionService DispatchService(ProviderType provider)
    {
        var service = kernel.GetRequiredService<IChatCompletionService>(provider.ToString());
        var serviceConfig = provider switch
        {
            ProviderType.AzureOpenAI => config.AzureOpenAI.ToAIServiceConfig(),
            _ => throw new NotSupportedException(),
        };

        service.Initialize(serviceConfig);
        return service;
    }

    private void PrintAssistantMessage(ChatMessage response)
    {
        _ = this;
        var text = response.Text;
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

        if (!string.IsNullOrEmpty(response.AuthorName))
        {
            panel.Header = new PanelHeader(response.AuthorName);
        }

        AnsiConsole.Write(panel);
    }
}
