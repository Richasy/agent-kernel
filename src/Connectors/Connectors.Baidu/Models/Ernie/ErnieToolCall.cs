// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Baidu.Models;

internal sealed class ErnieToolCall
{
    public required string Id { get; set; }

    public required string Type { get; set; }

    public required ErnieFunctionCall Function { get; set; }
}
