// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json;

namespace Richasy.AgentKernel.Connectors.Baidu.Models;

internal sealed class ErnieFunctionToolParameters
{
    public string Type { get; set; } = "object";

    public required IDictionary<string, JsonElement> Properties { get; set; }

    public required string[] Required { get; set; }
}
