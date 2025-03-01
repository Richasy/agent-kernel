// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Google.Models.Core;

internal sealed class GeminiFunctionToolParameters
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "object";

    [JsonPropertyName("properties")]
    public required IDictionary<string, JsonElement> Properties { get; set; }

    [JsonPropertyName("required")]
    public required string[] Required { get; set; }
}
