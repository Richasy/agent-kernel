// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

#define USE_SYSTEM_PROMPT

using Microsoft.Extensions.AI;
using Microsoft.Extensions.Hosting;
using Richasy.AgentKernel;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.Ali.Models;
using Richasy.AgentKernel.Connectors.Baidu.Models;
using Richasy.AgentKernel.Connectors.Tencent.Models;
using Richasy.AgentKernel.Connectors.ZhiPu.Models;
using Richasy.AgentKernel.Models;
using RichasyKernel;
using Spectre.Console;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Consoles.Chat;

#pragma warning disable CA1001 // 具有可释放字段的类型应该是可释放的
#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks
internal sealed class ChatService(Kernel kernel, IChatConfigManager configManager, IHostApplicationLifetime lifetime) : IHostedService
{
    private readonly CancellationTokenSource _stopCts = new();
    private Task? _chatTask;
    private ChatModel? _model;

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

    private ChatProviderType AskProvider()
    {
        var providers = Enum.GetValues<ChatProviderType>();
        return AnsiConsole.Prompt(new SelectionPrompt<ChatProviderType>()
            .Title("Select a provider")
            .PageSize(20)
            .MoreChoicesText("More")
            .UseConverter(ProviderToName)
            .AddChoices(providers));
    }

    private async Task<ChatModel?> AskModelAsync(ChatProviderType providerType)
    {
        var config = await configManager.GetChatConfigAsync(providerType);
        var models = kernel.GetRequiredService<IChatService>(providerType.ToString()).GetPredefinedModels().ToList();
        if (config?.IsCustomModelNotEmpty() ?? false)
        {
            models.AddRange(config.CustomModels!);
        }

        return models?.Count > 0
            ? AnsiConsole.Prompt(new SelectionPrompt<ChatModel>()
            .Title("Select a model")
            .PageSize(20)
            .MoreChoicesText("More")
            .UseConverter(x => x.Name)
            .AddChoices(models))
            : default;
    }

    private string ProviderToName(ChatProviderType provider)
    {
        return provider switch
        {
            ChatProviderType.OpenAI => "OpenAI",
            ChatProviderType.AzureOpenAI => "Azure OpenAI",
            ChatProviderType.AzureAI => "Azure AI",
            ChatProviderType.XAI => "xAI",
            ChatProviderType.ZhiPu => "智谱",
            ChatProviderType.LingYi => "零一万物",
            ChatProviderType.Anthropic => "Anthropic",
            ChatProviderType.Moonshot => "月之暗面",
            ChatProviderType.Gemini => "Gemini",
            ChatProviderType.DeepSeek => "DeepSeek",
            ChatProviderType.Qwen => "通义千问",
            ChatProviderType.Ernie => "文心一言",
            ChatProviderType.Hunyuan => "混元",
            ChatProviderType.Spark => "讯飞星火",
            ChatProviderType.Doubao => "豆包",
            ChatProviderType.SiliconFlow => "硅基流动",
            ChatProviderType.OpenRouter => "OpenRouter",
            ChatProviderType.TogetherAI => "Together.AI",
            ChatProviderType.Groq => "Groq",
            ChatProviderType.Mistral => "Mistral",
            ChatProviderType.Ollama => "Ollama",
            ChatProviderType.Perplexity => "Perplexity",
            ChatProviderType.Onnx => "ONNX",
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
            _model = await AskModelAsync(provider);
            var service = await DispatchServiceAsync(provider);
            var client = new ChatClientBuilder(service.Client!)
                .UseFunctionInvocation(configure: client => client.MaximumIterationsPerRequest = 2)
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
                var options = GetChatOptions(provider);
                options.ModelId = service.Config?.Model;

                if (_model?.ToolSupport ?? false)
                {
                    options.Tools = [
                        AIFunctionFactory.Create(
                             ([Description("The person whose age is being requested")] string personName) => "42岁", "GetPersonAge", "Gets the age of the specified person."),
                        // new ErnieWebSearchTool { Enable = true }
                        // new ZhiPuWebSearchTool { Enable = true }
                        // new ZhiPuRetrievalTool { KnowledgeId = "1871787212023255040", PromptTemplate = "从文档\n\"\"\"\n{{knowledge}}\n\"\"\"\n中找问题\n\"\"\"\n{{question}}\n\"\"\"\n的答案，找到答案就仅使用文档语句回答问题，找不到答案就用自身知识回答并且告诉用户该信息不是来自文档。\n不要复述问题，直接开始回答。"}
                    ];
                }

                // options.AdditionalProperties!.Add("visual", true);

                //await foreach (var message in client.CompleteStreamingAsync(chatMessages, options, cancellationToken: cancellationToken))
                //{
                //    System.Diagnostics.Debug.WriteLine(message.Text);
                //    responseMessage += message.Text;
                //}

                var response = await client.GetResponseAsync(chatMessages, options, cancellationToken: cancellationToken);
                if (response.Message.AdditionalProperties?.ContainsKey("reasoning_content") ?? false)
                {
                    var reasoningContent = response.Message.AdditionalProperties["reasoning_content"];
                    if (reasoningContent is BinaryData binaryData)
                    {
                        var content = binaryData.ToString();
                        content = JsonSerializer.Deserialize(content, JsonGenerationContext.Default.String);
                        if (content != null)
                        {
                            PrintReasoningMessage(content);
                        }
                    }
                }

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

    private async Task<IChatService> DispatchServiceAsync(ChatProviderType provider)
    {
        var config = await configManager.GetServiceConfigAsync(provider, _model!);
        var service = kernel.GetRequiredService<IChatService>(provider.ToString());
        service.Initialize(config!);
        return service;
    }

    private static ChatOptions GetChatOptions(ChatProviderType provider)
    {
        return provider switch
        {
            ChatProviderType.Qwen => new QwenChatOptions { EnableSearch = true },
            ChatProviderType.Ernie => new ErnieChatOptions { WebSearch = new ErnieWebSearchParameters { Enable = true, EnableTrace = true } },
            ChatProviderType.Hunyuan => new HunyuanChatOptions { EnableEnhancement = true, ForceSearchEnhancement = true },
            ChatProviderType.ZhiPu => new ZhiPuChatOptions { Search = new() { Enable = true } },
            _ => new ChatOptions(),
        };
    }

    private void PrintReasoningMessage(string text)
    {
        _ = this;
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

        panel.Header = new PanelHeader("Thinking...");
        AnsiConsole.Write(panel);
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
