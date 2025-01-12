// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.SiliconFlow;
using RichasyKernel;

namespace Richasy.AgentKernel;

/// <summary>
/// Provides extension methods for <see cref="Kernel"/>.
/// </summary>
public static class KernelExtensions
{
    /// <summary>
    /// 添加硅基流动对话服务.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddSiliconFlowChatCompletion(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatService, SiliconFlowChatService>("SiliconFlow");
        return builder;
    }

    /// <summary>
    /// 添加硅基流动模型提供程序.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddSiliconFlowChatModelProvider(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatModelProvider, SiliconFlowChatModelProvider>("SiliconFlow");
        return builder;
    }
}
