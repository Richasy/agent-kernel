// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel;
using Richasy.AgentKernel.Core.Mcp.Models;
using System.Text.Json.Serialization;

namespace Consoles.Chat;

[JsonSourceGenerationOptions(DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(ChatClientConfiguration))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(McpServerList))]
internal sealed partial class JsonGenerationContext : JsonSerializerContext
{
}
