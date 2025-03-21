// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.XAI;
using Richasy.AgentKernel.Draw;
using RichasyKernel;

namespace Richasy.AgentKernel;

/// <summary>
/// Provides extension methods for <see cref="Kernel"/>.
/// </summary>
public static class KernelExtensions
{
    /// <summary>
    /// Add xAI chat completion service.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddXAIChatService(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatService, XAIChatService>("XAI");
        return builder;
    }

    /// <summary>
    /// Adds a keyed singleton service for drawing functionality to the service collection.
    /// </summary>
    /// <returns>Returns the updated service builder for further configuration.</returns>
    public static IKernelBuilder AddXAIDrawService(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IDrawService, XAIDrawService>("XAI");
        return builder;
    }
}
