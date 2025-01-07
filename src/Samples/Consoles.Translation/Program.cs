// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Consoles.Translation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Richasy.AgentKernel;
using RichasyKernel;

ConfigureConsole();
await LoadConfigurationAsync().ConfigureAwait(true);
var builder = Host.CreateApplicationBuilder(args);
builder.Environment.ContentRootPath = AppDomain.CurrentDomain.BaseDirectory;
var kernel = Kernel.CreateBuilder()
    .AddAzureTranslationService()
    .AddAliTranslationService()
    .AddBaiduTranslationService()
    .AddTencentTranslationService()
    .Build();

builder.Services.AddSingleton(kernel);
builder.Services.AddSingleton(_config!);
builder.Services.AddHostedService<TranslationService>();

using var host = builder.Build();
await host.RunAsync().ConfigureAwait(true);
