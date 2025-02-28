// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Consoles.Chat;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Richasy.AgentKernel;
using RichasyKernel;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
var builder = Host.CreateApplicationBuilder(args);
builder.Environment.ContentRootPath = AppDomain.CurrentDomain.BaseDirectory;
var kernel = Kernel.CreateBuilder()
    .AddOpenAIChatService()
    .AddAzureOpenAIChatService()
    .AddAzureAIChatService()
    .AddXAIChatService()
    .AddZhiPuChatService()
    .AddLingYiChatService()
    .AddAnthropicChatService()
    .AddMoonshotChatService()
    .AddGeminiChatService()
    .AddDeepSeekChatService()
    .AddQwenChatService()
    .AddErnieChatService()
    .AddHunyuanChatService()
    .AddSparkChatService()
    .AddDoubaoChatService()
    .AddSiliconFlowChatService()
    .AddOpenRouterChatService()
    .AddTogetherAIChatService()
    .AddGroqChatService()
    .AddOllamaChatService()
    .AddMistralChatService()
    .AddOnnxChatService()
    .Build();

builder.Services.AddSingleton(kernel);
builder.Services.AddSingleton<IChatConfigManager, ChatConfigManager>();
builder.Services.AddHostedService<ChatService>();

using var host = builder.Build();
await host.RunAsync();