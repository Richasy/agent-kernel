// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.ZhiPu.Models;

internal sealed class ZhiPuToolCall
{
    public required string Id { get; set; }

    public required string Type { get; set; }

    public ZhiPuFunctionToolCall? Function { get; set; }
}
