// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.ChatCompletion;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.ZhiPu;

/// <summary>
/// Provides chat models for the ZhiPu connector.
/// </summary>
public sealed class ZhiPuChatModelProvider : IChatModelProvider
{
    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetModels() =>
    [
        new("glm-zero-preview", "GLM-Zero-Preview", toolSupport: true),
        new("glm-4-plus", "GLM-4-Plus", toolSupport: true),
        new("glm-4-0520", "GLM-4", toolSupport: true),
        new("glm-4-long", "GLM-4-Long", toolSupport: true),
        new("glm-4-airx", "GLM-4-AirX", toolSupport: true),
        new("glm-4-air", "GLM-4-Air", toolSupport: true),
        new("glm-4-flashx", "GLM-4-FlashX", toolSupport: true),
        new("glm-4-flash", "GLM-4-Flash", toolSupport: true),
        new("glm-4v-flash", "GLM-4V-Flash", visionSupport: true),
        new("glm-4v-plus", "GLM-4V-Plus", visionSupport: true),
        new("glm-4v", "GLM-4V", visionSupport: true),
    ];
}
