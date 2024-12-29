// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Richasy.AgentKernel.ChatCompletion;
using Richasy.AgentKernel.Connectors.Gemini;
using RichasyKernel;

namespace Richasy.AgentKernel;

/// <summary>
/// Provides extension methods for <see cref="Kernel"/>.
/// </summary>
public static class KernelExtensions
{
    /// <summary>
    /// Add Gemini chat completion service.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddGeminiChatCompletion(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatCompletionService, GeminiChatCompletionService>("Gemini");
        return builder;
    }
}
