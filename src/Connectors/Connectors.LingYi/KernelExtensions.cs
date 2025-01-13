// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.LingYi;
using RichasyKernel;

namespace Richasy.AgentKernel;

/// <summary>
/// Provides extension methods for <see cref="Kernel"/>.
/// </summary>
public static class KernelExtensions
{
    /// <summary>
    /// Add LingYi chat completion service.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddLingYiChatService(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatService, LingYiChatService>("LingYi");
        return builder;
    }

    /// <summary>
    /// Add LingYi chat model provider.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddLingYiChatModelProvider(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatModelProvider, LingYiChatModelProvider>("LingYi");
        return builder;
    }
}
