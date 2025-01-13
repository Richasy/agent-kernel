// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.Anthropic;
using RichasyKernel;

namespace Richasy.AgentKernel;

/// <summary>
/// Provides extension methods for <see cref="Kernel"/>.
/// </summary>
public static class KernelExtensions
{
    /// <summary>
    /// Add Anthropic chat completion service.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddAnthropicChatService(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatService, AnthropicChatService>("Anthropic");
        return builder;
    }

    /// <summary>
    /// Add Anthropic chat model provider.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddAnthropicChatModelProvider(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatModelProvider, AnthropicChatModelProvider>("Anthropic");
        return builder;
    }
}
