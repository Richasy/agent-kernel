// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.Baidu;
using Richasy.AgentKernel.Translation;
using RichasyKernel;

namespace Richasy.AgentKernel;

/// <summary>
/// Provides extension methods for <see cref="Kernel"/>.
/// </summary>
public static class KernelExtensions
{
    /// <summary>
    /// 添加文心一言对话服务.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddErnieChatCompletion(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatService, ErnieChatService>("Ernie");
        return builder;
    }

    /// <summary>
    /// 添加文心一言模型提供程序.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddErnieChatModelProvider(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatModelProvider, ErnieChatModelProvider>("Ernie");
        return builder;
    }

    /// <summary>
    /// 添加百度翻译服务.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddBaiduTranslationService(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<ITranslationService, BaiduTranslationService>("Baidu");
        return builder;
    }
}
