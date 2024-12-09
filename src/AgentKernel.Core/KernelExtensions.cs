// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Richasy.AgentKernel.ChatCompletion;

namespace Richasy.AgentKernel;

/// <summary>
/// Provides extension methods for <see cref="Kernel"/> and <see cref="IKernelBuilder"/>.
/// </summary>
public static class KernelExtensions
{
    /// <summary>
    /// Build the kernel.
    /// </summary>
    /// <param name="builder">Kernel builder.</param>
    /// <returns><see cref="Kernel"/>.</returns>
    /// <exception cref="InvalidOperationException">No services registered.</exception>
    public static Kernel Build(this IKernelBuilder builder)
    {
        return builder.Services is { Count: > 0 } services
            ? new Kernel(services.BuildServiceProvider())
            : throw new InvalidOperationException("No services registered.");
    }

    /// <summary>
    /// Get the chat completion service.
    /// </summary>
    /// <returns><see cref="IChatCompletionService"/>.</returns>
    public static IChatCompletionService GetChatCompletionService(this Kernel kernel, object? serviceKey = null)
        => kernel.GetRequiredService<IChatCompletionService>(serviceKey);
}
