// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.ZhiPu;
using Richasy.AgentKernel.Draw;
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
    public static IKernelBuilder AddZhiPuChatService(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatService, ZhiPuChatService>("ZhiPu");
        return builder;
    }

    /// <summary>
    /// Adds a keyed singleton service for drawing functionality to the service collection.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddZhiPuDrawService(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IDrawService, ZhiPuDrawService>("ZhiPu");
        return builder;
    }
}