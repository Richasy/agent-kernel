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
    .AddOpenAIChatCompletion()
    .AddAzureOpenAIChatCompletion()
    .AddXAIChatCompletion()
    .AddZhiPuChatCompletion()
    .AddLingYiChatCompletion()
    .AddAnthropicChatCompletion()
    .AddMoonshotChatCompletion()
    .AddGeminiChatCompletion()
    .AddDeepSeekChatCompletion()
    .AddQwenChatCompletion()
    .AddErnieChatCompletion()
    .AddHunyuanChatCompletion()
    .AddSparkChatCompletion()
    .AddDoubaoChatCompletion()
    .AddSiliconFlowChatCompletion()
    .AddOpenRouterChatCompletion()
    .AddTogetherAIChatCompletion()
    .AddGroqChatCompletion()
    .AddMistralChatCompletion()
    .AddOllamaChatCompletion()
    
    .AddQwenChatModelProvider()
    .AddAnthropicChatModelProvider()
    .AddAzureOpenAIChatModelProvider()
    .AddErnieChatModelProvider()
    .AddDeepSeekChatModelProvider()
    .AddGeminiChatModelProvider()
    .AddGroqChatModelProvider()
    .AddSparkChatModelProvider()
    .AddLingYiChatModelProvider()
    .AddMistralChatModelProvider()
    .AddMoonshotChatModelProvider()
    .AddOllamaChatModelProvider()
    .AddOpenAIChatModelProvider()
    .AddOpenRouterChatModelProvider()
    .AddSiliconFlowChatModelProvider()
    .AddHunyuanChatModelProvider()
    .AddTogetherAIChatModelProvider()
    .AddDoubaoChatModelProvider()
    .AddXAIChatModelProvider()
    .AddDoubaoChatModelProvider()
    .Build();

builder.Services.AddSingleton(kernel);
builder.Services.AddSingleton(_config!);
builder.Services.AddHostedService<ChatService>();

using var host = builder.Build();
await host.RunAsync();