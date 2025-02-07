// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.Tencent;
using Richasy.AgentKernel.Draw;
using Richasy.AgentKernel.Translation;
using RichasyKernel;

namespace Richasy.AgentKernel;

/// <summary>
/// Provides extension methods for <see cref="Kernel"/>.
/// </summary>
public static class KernelExtensions
{
    /// <summary>
    /// 添加腾讯混元对话服务.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddHunyuanChatService(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatService, HunyuanChatService>("Hunyuan");
        return builder;
    }

    /// <summary>
    /// 添加腾讯翻译服务.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddTencentTranslationService(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<ITranslateService, TencentTranslationService>("Tencent");
        return builder;
    }

    /// <summary>
    /// 添加腾讯混元绘图服务.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddHunyuanDrawService(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IDrawService, HunyuanDrawService>("Hunyuan");
        return builder;
    }
}
