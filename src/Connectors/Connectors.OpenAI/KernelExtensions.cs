// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Richasy.AgentKernel.Audio;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.OpenAI;
using Richasy.AgentKernel.Draw;
using RichasyKernel;

namespace Richasy.AgentKernel;

/// <summary>
/// Provides extension methods for <see cref="Kernel"/>.
/// </summary>
public static class KernelExtensions
{
    /// <summary>
    /// Add OpenAI chat completion service.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddOpenAIChatCompletion(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatService, OpenAIChatService>("OpenAI");
        return builder;
    }

    /// <summary>
    /// Add OpenAI chat model provider.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddOpenAIChatModelProvider(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatModelProvider, OpenAIChatModelProvider>("OpenAI");
        return builder;
    }

    /// <summary>
    /// Add OpenAI audio service.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddOpenAIAudioService(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IAudioService, OpenAIAudioService>("OpenAI");
        return builder;
    }

    /// <summary>
    /// Add OpenAI draw service.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddOpenAIDrawService(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IDrawService, OpenAIDrawService>("OpenAI");
        return builder;
    }
}
