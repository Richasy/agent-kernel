// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Core.Mcp.Internal;

internal class RpcResponse
{
    [JsonPropertyName("jsonrpc")]
    public string JsonRpc { get; set; }

    [JsonPropertyName("error")]
    public RpcError? Error { get; set; }

    [JsonPropertyName("id")]
    public int? Id { get; set; }
}

internal sealed class RpcResponseWith<T> : RpcResponse
{
    [JsonPropertyName("result")]
    public T? Result { get; set; }
}
