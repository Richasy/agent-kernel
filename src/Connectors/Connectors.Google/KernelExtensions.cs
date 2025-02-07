// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.Google;
using Richasy.AgentKernel.Translation;
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
    public static IKernelBuilder AddGeminiChatService(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatService, GeminiChatService>("Gemini");
        return builder;
    }

    /// <summary>
    /// Add Google translation service.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddGoogleTranslationService(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<ITranslateService, GoogleTranslationService>("Google");
        return builder;
    }
}
