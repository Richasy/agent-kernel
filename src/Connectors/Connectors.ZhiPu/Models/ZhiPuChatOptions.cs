// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;

namespace Richasy.AgentKernel.Connectors.ZhiPu.Models;

/// <summary>
/// 智谱聊天选项.
/// </summary>
public sealed class ZhiPuChatOptions : ChatOptions
{
    /// <summary>
    /// 是否支持视觉解析.
    /// </summary>
    public bool VisionSupport { get; set; }
}
