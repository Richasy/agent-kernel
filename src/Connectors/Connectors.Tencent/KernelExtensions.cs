// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Richasy.AgentKernel.ChatCompletion;
using Richasy.AgentKernel.Connectors.Tencent;
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
    public static IKernelBuilder AddHunyuanChatCompletion(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatCompletionService, HunyuanChatCompletionService>("Hunyuan");
        return builder;
    }

    /// <summary>
    /// 添加腾讯混元模型提供程序.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddHunyuanChatModelProvider(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatModelProvider, HunyuanChatModelProvider>("Hunyuan");
        return builder;
    }

    /// <summary>
    /// 添加腾讯翻译服务.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddTencentTranslationService(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<ITextTranslationService, TencentTranslationService>("Tencent");
        return builder;
    }
}
