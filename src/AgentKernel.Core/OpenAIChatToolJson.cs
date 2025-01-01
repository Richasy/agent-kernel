// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;
using System.Text.Json;

namespace Richasy.AgentKernel;

/// <summary>
/// OpenAI Chat Tool JSON.
/// </summary>
public sealed class OpenAIChatToolJson
{
    /// <summary>Gets a singleton JSON data for empty parameters. Optimization for the reasonably common case of a parameterless function.</summary>
    public static BinaryData ZeroFunctionParametersSchema { get; } = new("""{"type":"object","required":[],"properties":{}}"""u8.ToArray());

    /// <summary>
    /// Gets a singleton JSON data for empty parameters. Optimization for the reasonably common case of a parameterless function.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "object";

    /// <summary>
    /// Gets or sets the required properties.
    /// </summary>
    [JsonPropertyName("required")]
    public HashSet<string> Required { get; } = [];

    /// <summary>
    /// Gets or sets the properties.
    /// </summary>
    [JsonPropertyName("properties")]
    public Dictionary<string, JsonElement> Properties { get; } = [];
}
