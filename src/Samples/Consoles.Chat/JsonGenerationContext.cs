// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel;
using Richasy.AgentKernel.Core.Mcp.Shared;
using System.Text.Json.Serialization;

namespace Consoles.Chat;

[JsonSerializable(typeof(ChatClientConfiguration))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(McpServerDefinitionCollection))]
internal sealed partial class JsonGenerationContext : JsonSerializerContext
{
}
