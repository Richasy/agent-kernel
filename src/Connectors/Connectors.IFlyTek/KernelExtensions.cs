// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.IFlyTek;
using RichasyKernel;

namespace Richasy.AgentKernel;

/// <summary>
/// Provides extension methods for <see cref="Kernel"/>.
/// </summary>
public static class KernelExtensions
{
    /// <summary>
    /// 添加讯飞星火对话服务.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddSparkChatCompletion(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatService, SparkChatService>("Spark");
        return builder;
    }

    /// <summary>
    /// 添加讯飞星火模型提供程序.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddSparkChatModelProvider(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatModelProvider, SparkChatModelProvider>("Spark");
        return builder;
    }
}
