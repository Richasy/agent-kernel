// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Richasy.AgentKernel.Audio;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.Azure;
using Richasy.AgentKernel.Draw;
using Richasy.AgentKernel.Translation;
using RichasyKernel;

namespace Richasy.AgentKernel;

/// <summary>
/// Provides extension methods for <see cref="Kernel"/>.
/// </summary>
public static class KernelExtensions
{
    /// <summary>
    /// Add Azure OpenAI chat completion service.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddAzureOpenAIChatCompletion(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatService, AzureOpenAIChatService>("AzureOpenAI");
        return builder;
    }

    /// <summary>
    /// Add Azure OpenAI chat model provider.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddAzureOpenAIChatModelProvider(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatModelProvider, AzureOpenAIChatModelProvider>("AzureOpenAI");
        return builder;
    }

    /// <summary>
    /// Add Azure translation service.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddAzureTranslationService(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<ITranslationService, AzureTranslationService>("Azure");
        return builder;
    }

    /// <summary>
    /// Add Azure audio service.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddAzureAudioService(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IAudioService, AzureAudioService>("Azure");
        return builder;
    }

    /// <summary>
    /// Add Edge audio service.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddEdgeAudioService(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IAudioService, EdgeAudioService>("Edge");
        return builder;
    }

    /// <summary>
    /// Add Azure OpenAI audio service.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddAzureOpenAIAudioService(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IAudioService, AzureOpenAIAudioService>("AzureOpenAI");
        return builder;
    }

    /// <summary>
    /// Add Azure OpenAI draw service.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddAzureOpenAIDrawService(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IDrawService, AzureOpenAIDrawService>("AzureOpenAI");
        return builder;
    }
}
