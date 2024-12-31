// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

#define USE_SYSTEM_PROMPT

using Connectors.DeepSeek.Models;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Hosting;
using Richasy.AgentKernel.ChatCompletion;
using Richasy.AgentKernel.Connectors.Anthropic.Models;
using Richasy.AgentKernel.Connectors.AzureOpenAI.Models;
using Richasy.AgentKernel.Connectors.Baidu.Models;
using Richasy.AgentKernel.Connectors.Gemini.Models;
using Richasy.AgentKernel.Connectors.IFlyTek.Models;
using Richasy.AgentKernel.Connectors.LingYi.Models;
using Richasy.AgentKernel.Connectors.Moonshot.Models;
using Richasy.AgentKernel.Connectors.OpenRouter.Models;
using Richasy.AgentKernel.Connectors.SiliconFlow.Models;
using Richasy.AgentKernel.Connectors.Tencent.Models;
using Richasy.AgentKernel.Connectors.Volcano.Models;
using Richasy.AgentKernel.Connectors.XAI.Models;
using Richasy.AgentKernel.Connectors.ZhiPu.Models;
using RichasyKernel;
using Spectre.Console;
using System.ComponentModel;
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
            .PageSize(20)
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
            ProviderType.LingYi => "零一万物",
            ProviderType.Anthropic => "Anthropic",
            ProviderType.Moonshot => "月之暗面",
            ProviderType.Gemini => "Gemini",
            ProviderType.DeepSeek => "DeepSeek",
            ProviderType.Qwen => "通义千问",
            ProviderType.Ernie => "文心一言",
            ProviderType.Hunyuan => "混元",
            ProviderType.Spark => "讯飞星火",
            ProviderType.Doubao => "豆包",
            ProviderType.SiliconFlow => "硅基流动",
            ProviderType.OpenRouter => "OpenRouter",
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
                // var userMessage = new ChatMessage(ChatRole.User, [new VideoContent("https://sfile.chatglm.cn/testpath/video/b844f8f1-5df9-556c-a515-3d3bfaa736e8_0.mp4"), new TextContent(userPrompt)]);
                // chatMessages.Add(userMessage);
                // var response = await service.Client!.CompleteAsync(chatMessages, cancellationToken: cancellationToken);
                // var responseMessage = response.Message.Text;
                var responseMessage = string.Empty;
                var options = new ChatOptions()
                {
                    Tools =
                    [
                         AIFunctionFactory.Create(
                             ([Description("The person whose age is being requested")] string personName) => 42, "GetPersonAge", "Gets the age of the specified person."),
                        // new ZhiPuWebSearchTool { Enable = true }
                        // new ZhiPuRetrievalTool { KnowledgeId = "1871787212023255040", PromptTemplate = "从文档\n\"\"\"\n{{knowledge}}\n\"\"\"\n中找问题\n\"\"\"\n{{question}}\n\"\"\"\n的答案，找到答案就仅使用文档语句回答问题，找不到答案就用自身知识回答并且告诉用户该信息不是来自文档。\n不要复述问题，直接开始回答。"}
                    ],

                    AdditionalProperties = [],
                };

                // options.AdditionalProperties!.Add("visual", true);

                //await foreach (var message in client.CompleteStreamingAsync(chatMessages, options, cancellationToken: cancellationToken))
                //{
                //    System.Diagnostics.Debug.WriteLine(message.Text);
                //    responseMessage += message.Text;
                //}
                var response = await client.CompleteAsync(chatMessages, options, cancellationToken: cancellationToken);
                responseMessage = response.Message.Text;
                chatMessages.Add(new ChatMessage(ChatRole.Assistant, responseMessage?.Trim()));
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
            ProviderType.AzureOpenAI => config.AzureOpenAI.ToAIServiceConfig<AzureOpenAIServiceConfig>(),
            ProviderType.XAI => config.XAI.ToAIServiceConfig<XAIServiceConfig>(),
            ProviderType.ZhiPu => config.ZhiPu.ToAIServiceConfig<ZhiPuServiceConfig>(),
            ProviderType.LingYi => config.LingYi.ToAIServiceConfig<LingYiServiceConfig>(),
            ProviderType.Anthropic => config.Anthropic.ToAIServiceConfig<AnthropicServiceConfig>(),
            ProviderType.Moonshot => config.Moonshot.ToAIServiceConfig<MoonshotServiceConfig>(),
            ProviderType.Gemini => config.Gemini.ToAIServiceConfig<GeminiServiceConfig>(),
            ProviderType.DeepSeek => config.DeepSeek.ToAIServiceConfig<DeepSeekServiceConfig>(),
            ProviderType.Qwen => config.Qwen.ToAIServiceConfig<DeepSeekServiceConfig>(),
            ProviderType.Ernie => config.Ernie.ToAIServiceConfig<ErnieServiceConfig>(),
            ProviderType.Hunyuan => config.Hunyuan.ToAIServiceConfig<HunyuanServiceConfig>(),
            ProviderType.Spark => config.Spark.ToAIServiceConfig<SparkServiceConfig>(),
            ProviderType.Doubao => config.Doubao.ToAIServiceConfig<DoubaoServiceConfig>(),
            ProviderType.SiliconFlow => config.SiliconFlow.ToAIServiceConfig<SiliconFlowServiceConfig>(),
            ProviderType.OpenRouter => config.OpenRouter.ToAIServiceConfig<OpenRouterServiceConfig>(),
            _ => throw new NotSupportedException(),
        } ?? throw new InvalidOperationException("The configuration is not valid.");
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
