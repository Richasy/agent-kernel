// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Baidu.Models;

internal sealed class ErnieFunction
{
    public required string Name { get; set; }

    public string? Description { get; set; }

    public ErnieFunctionToolParameters? Parameters { get; set; }
}