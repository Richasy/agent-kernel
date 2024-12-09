// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Consoles.Chat;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Richasy.AgentKernel;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
ConfigureConsole();
await LoadConfigurationAsync();
var builder = Host.CreateApplicationBuilder(args);
builder.Environment.ContentRootPath = AppDomain.CurrentDomain.BaseDirectory;
var kernel = Kernel.CreateBuilder()
    .AddAzureOpenAIChatCompletion()
    .Build();

builder.Services.AddSingleton(kernel);
builder.Services.AddSingleton(_config!);
builder.Services.AddHostedService<ChatService>();

using var host = builder.Build();
await host.RunAsync();
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释