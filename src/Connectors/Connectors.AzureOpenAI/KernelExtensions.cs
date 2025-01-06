// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Richasy.AgentKernel.ChatCompletion;
using Richasy.AgentKernel.Connectors.Azure;
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
        builder.Services.AddKeyedSingleton<IChatCompletionService, AzureOpenAIChatCompletionService>("AzureOpenAI");
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
        builder.Services.AddKeyedSingleton<ITextTranslationService, AzureTranslationService>("Azure");
        builder.Services.AddKeyedSingleton<IHtmlTranslationService, AzureTranslationService>("Azure");
        return builder;
    }
}
