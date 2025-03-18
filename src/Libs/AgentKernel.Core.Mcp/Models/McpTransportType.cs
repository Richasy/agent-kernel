using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Richasy.AgentKernel.Core.Mcp.Models;

/// <summary>
/// MCP 传输类型.
/// </summary>
public enum McpTransportType
{
    /// <summary>
    /// 标准输入输出. 适用于本地进程.
    /// </summary>
    StandardInputOutput,

    /// <summary>
    /// HTTP Server-Sent Events. 适用于远程进程.
    /// </summary>
    ServerSentEvents,
}
