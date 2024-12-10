// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json;

namespace Richasy.AgentKernel.Connectors.ZhiPu.Models;

internal sealed class ZhiPuFunctionToolCall
{
    public required string Name { get; set; }

    public JsonElement? Arguments { get; set; }
}
