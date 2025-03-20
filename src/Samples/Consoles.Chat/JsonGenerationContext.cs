// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel;
using Richasy.AgentKernel.Core.Mcp.Protocol.Types;
using Richasy.AgentKernel.Core.Mcp.Shared;
using System.Text.Json.Serialization;

namespace Consoles.Chat;

[JsonSourceGenerationOptions(DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(ChatClientConfiguration))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(McpTool))]
[JsonSerializable(typeof(McpServerDefinitionCollection))]
internal sealed partial class JsonGenerationContext : JsonSerializerContext
{
}
