// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.Perplexity;
using RichasyKernel;

namespace Richasy.AgentKernel;

/// <summary>
/// Provides extension methods for <see cref="Kernel"/>.
/// </summary>
public static class KernelExtensions
{
    /// <summary>
    /// 添加 Perplexity 对话服务.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddPerplexityChatCompletion(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatService, PerplexityChatService>("Perplexity");
        return builder;
    }

    /// <summary>
    /// 添加 Perplexity 对话模型提供程序.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddPerplexityChatModelProvider(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatModelProvider, PerplexityChatModelProvider>("Perplexity");
        return builder;
    }
}
