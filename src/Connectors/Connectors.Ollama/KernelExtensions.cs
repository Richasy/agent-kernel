// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.Ollama;
using RichasyKernel;

namespace Richasy.AgentKernel;

/// <summary>
/// Provides extension methods for <see cref="Kernel"/>.
/// </summary>
public static class KernelExtensions
{
    /// <summary>
    /// Add Ollama chat completion service.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddOllamaChatService(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatService, OllamaChatService>("Ollama");
        return builder;
    }

    /// <summary>
    /// Add Ollama chat model provider.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddOllamaChatModelProvider(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatModelProvider, OllamaChatModelProvider>("Ollama");
        return builder;
    }
}
