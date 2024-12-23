// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

#define USE_SYSTEM_PROMPT

using Microsoft.Extensions.AI;
using Microsoft.Extensions.Hosting;
using Richasy.AgentKernel.ChatCompletion;
using RichasyKernel;
using Spectre.Console;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

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
            ProviderType.OpenAI => "OpenAI",
            ProviderType.AzureOpenAI => "Azure OpenAI",
            ProviderType.XAI => "xAI",
            ProviderType.ZhiPu => "智谱",
            _ => throw new NotSupportedException(),
        };
    }

    [UnconditionalSuppressMessage("AOT", "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.", Justification = "<Pending>")]
    private async Task RunChatAsync(CancellationToken cancellationToken)
    {
        try
        {
            AnsiConsole.Clear();
            var provider = AskProvider();
            var service = DispatchService(provider);
            var client = new ChatClientBuilder(service.Client!)
                .UseFunctionInvocation()
                .Build();
            List<ChatMessage> chatMessages = [];
#if USE_SYSTEM_PROMPT
            var sysPrompt = AnsiConsole.Prompt(
                new TextPrompt<string>("System prompt [green](Optional)[/]: ")
                .AllowEmpty());

            if (!string.IsNullOrEmpty(sysPrompt))
            {
                chatMessages.Add(new ChatMessage(ChatRole.System, sysPrompt));
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
                else if (userPrompt == "clear")
                {
                    chatMessages.RemoveAll(x => x.Role != ChatRole.System);
                    AnsiConsole.Clear();
                    continue;
                }

                chatMessages.Add(new ChatMessage(ChatRole.User, userPrompt));
                // var response = await service.Client!.CompleteAsync(chatMessages, cancellationToken: cancellationToken);
                // var responseMessage = response.Message.Text;
                var responseMessage = string.Empty;
                var options = new ChatOptions()
                {
                    Tools = [AIFunctionFactory.Create(
                        ([Description("The person whose age is being requested")] string personName) => 42, "GetPersonAge", "Gets the age of the specified person.")],
                };
                await foreach (var message in client.CompleteStreamingAsync(chatMessages, options, cancellationToken: cancellationToken))
                {
                    Debug.WriteLine(message.Text);
                    responseMessage += message.Text;
                }

                chatMessages.Add(new ChatMessage(ChatRole.Assistant, responseMessage));
                PrintAssistantMessage(chatMessages.Last());
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex);
            throw;
        }
    }

    private IChatCompletionService DispatchService(ProviderType provider)
    {
        var service = kernel.GetRequiredService<IChatCompletionService>(provider.ToString());
        var serviceConfig = provider switch
        {
            ProviderType.OpenAI => config.OpenAI.ToAIServiceConfig(),
            ProviderType.AzureOpenAI => config.AzureOpenAI.ToAIServiceConfig(),
            ProviderType.XAI => config.XAI.ToAIServiceConfig(),
            ProviderType.ZhiPu => config.ZhiPu.ToAIServiceConfig(),
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
