// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;

namespace Richasy.AgentKernel.Connectors.ZhiPu.Models;

/// <summary>
/// 智谱检索工具.
/// </summary>
public sealed class ZhiPuRetrievalTool : AITool
{
    /// <summary>
    /// 知识库Id.
    /// </summary>
    public required string KnowledgeId { get; set; }

    /// <summary>
    /// 提示词模板.
    /// </summary>
    public string? PromptTemplate { get; set; }
}
