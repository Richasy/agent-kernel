// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.Ali;
using Richasy.AgentKernel.Translation;
using RichasyKernel;

namespace Richasy.AgentKernel;

/// <summary>
/// Provides extension methods for <see cref="Kernel"/>.
/// </summary>
public static class KernelExtensions
{
    /// <summary>
    /// 添加通义千问对话服务.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddQwenChatCompletion(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatService, QwenChatService>("Qwen");
        return builder;
    }

    /// <summary>
    /// 添加通义千问模型提供程序.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddQwenChatModelProvider(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatModelProvider, QwenChatModelProvider>("Qwen");
        return builder;
    }

    /// <summary>
    /// Add Ali translation service.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddAliTranslationService(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<ITranslationService, AliTranslationService>("Ali");
        return builder;
    }
}
