using Richasy.AgentKernel.Core.Mcp.Protocol.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Richasy.AgentKernel.Core.Mcp;

/// <summary>
/// Global handler for MCP.
/// </summary>
public static class McpGlobalHandler
{
    /// <summary>
    /// Used to handle the consent of the user.
    /// </summary>
    /// <remarks>
    /// Input value: Client id, Method name, Request message.
    /// </remarks>
    public static Func<string, string, JsonRpcRequest, Task<bool>>? ConsentHandler { get; set; }

    /// <summary>
    /// Used to handle the consent of the user.
    /// </summary>
    public static List<string>? ToolCallWhiteList { get; set; }
}
