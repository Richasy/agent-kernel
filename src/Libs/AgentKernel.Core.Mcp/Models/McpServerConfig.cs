// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Core.Mcp.Models;

/// <summary>
/// MCP 服务器配置.
/// </summary>
public sealed class McpServerConfig
{
    /// <summary>
    /// 命令.
    /// </summary>
    [JsonPropertyName("command")]
    public string? Command { get; set; }

    /// <summary>
    /// 参数.
    /// </summary>
    [JsonPropertyName("args")]
    public string[]? Arguments { get; set; }

    /// <summary>
    /// 环境变量.
    /// </summary>
    [JsonPropertyName("env")]
    public Dictionary<string, string>? Environments { get; set; }

    /// <summary>
    /// 工作目录.
    /// </summary>
    [JsonPropertyName("workDir")]
    public string? WorkingDirectory { get; set; }

    /// <summary>
    /// 初始化 <see cref="McpServerConfig"/> 类的新实例.
    /// </summary>
    public McpServerConfig()
    {
    }

    /// <summary>
    /// 初始化 <see cref="McpServerConfig"/> 类的新实例.
    /// </summary>
    public McpServerConfig(
        string command,
        string[]? arguments = null,
        Dictionary<string, string>? environments = null,
        string? workDir = null)
    {
        Command = command;
        Arguments = arguments;
        Environments = environments;
        WorkingDirectory = workDir;
    }
}
