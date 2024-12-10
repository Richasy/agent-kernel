// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json;

namespace Richasy.AgentKernel.Connectors.ZhiPu.Models;

internal sealed class ZhiPuFunctionToolParameters
{
    public string Type { get; set; } = "object";

    public required IDictionary<string, JsonElement> Properties { get; set; }

    public required string[] Required { get; set; }
}
