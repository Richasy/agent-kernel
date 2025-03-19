using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Core.Mcp.Shared;

/// <summary>
/// Config definition for an MCP server.
/// </summary>
public sealed class McpServerDefinition
{
    /// <summary>
    /// The command to run.
    /// </summary>
    [JsonPropertyName("command")]
    public string? Command { get; set; }

    /// <summary>
    /// Arguments to pass to the command.
    /// </summary>
    [JsonPropertyName("args")]
    public string[]? Arguments { get; set; }

    /// <summary>
    /// Environment variables to set.
    /// </summary>
    [JsonPropertyName("env")]
    public Dictionary<string, string> Environments { get; set; }

    /// <summary>
    /// The working directory for the command.
    /// </summary>
    [JsonPropertyName("workDir")]
    public string? WorkingDirectory { get; set; }
}

/// <summary>
/// Collection of server definitions.
/// </summary>
public sealed class McpServerDefinitionCollection : Dictionary<string, McpServerDefinition>;