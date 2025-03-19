using Richasy.AgentKernel.Core.Mcp.Utils.Json;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Core.Mcp.Protocol.Messages;

/// <summary>
/// Base interface for all JSON-RPC messages in the MCP protocol.
/// </summary>
[JsonConverter(typeof(JsonRpcMessageConverter))]
public interface IJsonRpcMessage
{
    /// <summary>
    /// JSON-RPC protocol version. Must be "2.0".
    /// </summary>
    string JsonRpc { get; }
}
