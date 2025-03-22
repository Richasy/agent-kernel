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
    /// A static property that holds a function for handling JSON-RPC responses.
    /// </summary>
    public static Func<string, string, string, Task>? ResponseHandler { get; set; }

    /// <summary>
    /// Used to handle the consent of the user.
    /// </summary>
    public static List<string>? ToolCallWhiteList { get; set; }

    /// <summary>
    /// Represents the timeout duration for reading messages.
    /// </summary>
    public static TimeSpan? ReadMessageTimeOut { get; set; }
}
