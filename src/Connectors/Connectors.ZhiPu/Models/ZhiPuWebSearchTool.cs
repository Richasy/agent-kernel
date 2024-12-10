// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.ZhiPu.Models;

internal sealed class ZhiPuWebSearchTool
{
    public bool? Enable { get; set; }

    public string? SearchQuery { get; set; }

    public bool? SearchResult { get; set; }
}
