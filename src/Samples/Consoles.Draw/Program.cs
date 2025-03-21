// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Consoles.Draw;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Richasy.AgentKernel;
using RichasyKernel;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
var builder = Host.CreateApplicationBuilder(args);
builder.Environment.ContentRootPath = AppDomain.CurrentDomain.BaseDirectory;
var kernel = Kernel.CreateBuilder()
    .AddOpenAIDrawService()
    .AddAzureOpenAIDrawService()
    .AddErnieDrawService()
    .AddHunyuanDrawService()
    .AddSparkDrawService()
    .AddXAIDrawService()
    .AddZhiPuDrawService()
    .Build();

builder.Services.AddSingleton(kernel);
builder.Services.AddSingleton<IDrawConfigManager, DrawConfigManager>();
builder.Services.AddHostedService<DrawService>();

using var host = builder.Build();
await host.RunAsync().ConfigureAwait(true);
