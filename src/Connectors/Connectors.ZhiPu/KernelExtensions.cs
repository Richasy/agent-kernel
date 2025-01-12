// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.ZhiPu;
using RichasyKernel;

namespace Richasy.AgentKernel;

/// <summary>
/// Provides extension methods for <see cref="Kernel"/>.
/// </summary>
public static class KernelExtensions
{
    /// <summary>
    /// Add ZhiPu chat completion service.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddZhiPuChatCompletion(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatService, ZhiPuChatService>("ZhiPu");
        return builder;
    }

    /// <summary>
    /// Add ZhiPu chat model provider.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddZhiPuChatModelProvider(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatModelProvider, ZhiPuChatModelProvider>("ZhiPu");
        return builder;
    }
}