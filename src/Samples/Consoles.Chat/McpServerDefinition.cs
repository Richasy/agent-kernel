// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Consoles.Chat;

internal sealed class McpServerDefinition
{
    [JsonPropertyName("command")]
    public string? Command { get; set; }

    [JsonPropertyName("args")]
    public string[]? Arguments { get; set; }

    [JsonPropertyName("env")]
    public Dictionary<string, string>? EnvironmentVariables { get; set; }
}

internal sealed class McpServers
{
    [JsonPropertyName("mcpServers")]
    public Dictionary<string, McpServerDefinition>? Servers { get; set; }
}
