// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Richasy.AgentKernel.ChatCompletion;
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
    public static IKernelBuilder AddOllamaChatCompletion(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatCompletionService, OllamaChatCompletionService>("Ollama");
        return builder;
    }
}
