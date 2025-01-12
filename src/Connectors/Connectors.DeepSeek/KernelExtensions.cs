// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.DeepSeek;
using RichasyKernel;

namespace Richasy.AgentKernel;

/// <summary>
/// Provides extension methods for <see cref="Kernel"/>.
/// </summary>
public static class KernelExtensions
{
    /// <summary>
    /// Add DeepSeek chat completion service.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddDeepSeekChatCompletion(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatService, DeepSeekChatService>("DeepSeek");
        return builder;
    }

    /// <summary>
    /// Add DeepSeek chat model provider.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddDeepSeekChatModelProvider(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatModelProvider, DeepSeekChatModelProvider>("DeepSeek");
        return builder;
    }
}
