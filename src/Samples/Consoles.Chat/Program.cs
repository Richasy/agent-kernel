// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Consoles.Chat;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Richasy.AgentKernel;
using RichasyKernel;

ConfigureConsole();
await LoadConfigurationAsync();
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
    .Build();

builder.Services.AddSingleton(kernel);
builder.Services.AddSingleton(_config!);
builder.Services.AddHostedService<ChatService>();

using var host = builder.Build();
await host.RunAsync();