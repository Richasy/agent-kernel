// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.ZhiPu.Models;

internal sealed class ZhiPuFunctionToolCall
{
    public required string Name { get; set; }

    public string? Arguments { get; set; }
}
