// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.Onnx;
using RichasyKernel;

namespace Richasy.AgentKernel;

/// <summary>
/// Provides extension methods for <see cref="Kernel"/>.
/// </summary>
public static class KernelExtensions
{
    /// <summary>
    /// Add ONNX chat completion service.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddOnnxChatService(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatService, OnnxChatService>("Onnx");
        return builder;
    }
}
