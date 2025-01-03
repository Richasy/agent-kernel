// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Models;

/// <summary>
/// 聊天模型.
/// </summary>
public class ChatModel(string id, string name, bool toolSupport = false, bool visionSupport = false)
{
    /// <summary>
    /// 模型标识符.
    /// </summary>
    public string Id { get; set; } = id;

    /// <summary>
    /// 模型名称.
    /// </summary>
    public string Name { get; set; } = name;

    /// <summary>
    /// 是否支持工具调用.
    /// </summary>
    public bool ToolSupport { get; set; } = toolSupport;

    /// <summary>
    /// 是否支持图像视觉解析.
    /// </summary>
    public bool VisionSupport { get; set; } = visionSupport;
}
