// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Richasy.AgentKernel.Audio;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.Windows;
using RichasyKernel;

namespace Richasy.AgentKernel;

/// <summary>
/// Kernel extensions.
/// </summary>
public static class KernelExtensions
{
    /// <summary>
    /// Add Windows audio service.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddWindowsAudioService(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IAudioService, WindowsAudioService>("Windows");
        return builder;
    }

    /// <summary>
    /// Add Windows chat completion service.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddWindowsChatService(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatService, WindowsChatService>("Windows");
        return builder;
    }
}
