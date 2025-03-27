// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Richasy.AgentKernel.Audio;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.Volcano;
using Richasy.AgentKernel.Translation;
using RichasyKernel;

namespace Richasy.AgentKernel;

/// <summary>
/// Provides extension methods for <see cref="Kernel"/>.
/// </summary>
public static class KernelExtensions
{
    /// <summary>
    /// 添加字节豆包对话服务.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddDoubaoChatService(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IChatService, DoubaoChatService>("Doubao");
        return builder;
    }

    /// <summary>
    /// 添加火山翻译服务.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddVolcanoTranslationService(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<ITranslateService, VolcanoTranslationService>("Volcano");
        return builder;
    }

    /// <summary>
    /// 添加火山音频服务.
    /// </summary>
    /// <returns><see cref="IKernelBuilder"/>.</returns>
    public static IKernelBuilder AddVolcanoAudioService(this IKernelBuilder builder)
    {
        builder.Services.AddKeyedSingleton<IAudioService, VolcanoAudioService>("Volcano");
        return builder;
    }
}