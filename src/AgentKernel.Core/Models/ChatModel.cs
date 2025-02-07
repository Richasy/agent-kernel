// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Models;

/// <summary>
/// 聊天模型.
/// </summary>
public class ChatModel
{
    /// <summary>
    /// 初始化 <see cref="ChatModel"/> 类的新实例.
    /// </summary>
    public ChatModel()
    {
        Id = string.Empty;
        Name = string.Empty;
    }

    /// <summary>
    /// 初始化 <see cref="ChatModel"/> 类的新实例.
    /// </summary>
    public ChatModel(string id, string name, bool toolSupport = false, bool visionSupport = false)
    {
        Id = id;
        Name = name;
        ToolSupport = toolSupport;
        VisionSupport = visionSupport;
    }

    /// <summary>
    /// 模型标识符.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    /// 模型名称.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// 是否支持工具调用.
    /// </summary>
    [JsonPropertyName("tool_support")]
    public bool ToolSupport { get; set; }

    /// <summary>
    /// 是否支持图像视觉解析.
    /// </summary>
    [JsonPropertyName("vision_support")]
    public bool VisionSupport { get; set; }
}
