// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.ZhiPu.Models;

internal sealed class ZhiPuFunctionTool
{
    public required string Name { get; set; }

    public required string Description { get; set; }

    public required ZhiPuFunctionToolParameters Parameters { get; set; }
}
