// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.ZhiPu.Models;

internal sealed class ZhiPuTool
{
    public required string Type { get; set; }

    public ZhiPuFunctionTool? Function { get; set; }

    public ZhiPuRetrievalParameters? Retrieval { get; set; }

    public ZhiPuWebSearchParameters? WebSearch { get; set; }
}
