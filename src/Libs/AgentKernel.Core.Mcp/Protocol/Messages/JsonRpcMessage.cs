using Richasy.AgentKernel.Core.Mcp.Utils.Json;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Core.Mcp.Protocol.Messages;

/// <summary>
/// A request message in the JSON-RPC protocol.
/// </summary>
public record JsonRpcRequest : IJsonRpcMessageWithId
{
    /// <summary>
    /// JSON-RPC protocol version. Always "2.0".
    /// </summary>
    [JsonPropertyName("jsonrpc")]
    public string JsonRpc { get; init; } = "2.0";

    /// <summary>
    /// Request identifier. Must be a string or number and unique within the session.
    /// </summary>
    [JsonPropertyName("id")]
    public RequestId Id { get; set; }

    /// <summary>
    /// Name of the method to invoke.
    /// </summary>
    [JsonPropertyName("method")]
    public required string Method { get; init; }

    /// <summary>
    /// Optional parameters for the method.
    /// </summary>
    [JsonPropertyName("params")]
    [JsonConverter(typeof(BinaryJsonSchemaConverter))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BinaryData? Params { get; init; }
}
