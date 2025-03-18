// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Core.Mcp.Internal;
using Richasy.AgentKernel.Core.Mcp.Models;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Core.Mcp;

[JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, NumberHandling = JsonNumberHandling.AllowReadingFromString)]
[JsonSerializable(typeof(McpServerConfig))]
[JsonSerializable(typeof(RpcRequest))]
[JsonSerializable(typeof(RpcResponse))]
internal sealed partial class JsonGenContext : JsonSerializerContext
{
}
