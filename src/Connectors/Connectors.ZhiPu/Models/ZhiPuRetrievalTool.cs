// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.ZhiPu.Models;

internal sealed class ZhiPuRetrievalTool
{
    public required string KnowledgeId { get; set; }

    public string? PromptTemplate { get; set; }
}
