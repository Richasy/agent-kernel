// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Baidu.Models;

internal sealed class ErnieFunctionCall
{
    public required string Name { get; set; }

    public string? Arguments { get; set; }
}
