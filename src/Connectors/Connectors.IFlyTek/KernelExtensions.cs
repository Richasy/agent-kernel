// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.IFlyTek;
using Richasy.AgentKernel.Draw;
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
    public static IKernelBuilder AddSparkChatService(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatService, SparkChatService>("Spark");
        return builder;
    }

    /// <summary>
    /// 添加讯飞星火绘图服务.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddSparkDrawService(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IDrawService, SparkDrawService>("Spark");
        return builder;
    }
}
