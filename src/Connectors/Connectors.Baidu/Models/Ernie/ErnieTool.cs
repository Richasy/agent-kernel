// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Baidu.Models;

internal sealed class ErnieTool
{
    public required string Type { get; set; } = "function";

    public required ErnieFunction Function { get; set; }
}
