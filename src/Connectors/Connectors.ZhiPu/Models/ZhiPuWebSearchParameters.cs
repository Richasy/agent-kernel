// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.ZhiPu.Models;

/// <summary>
/// 智谱清言网络搜索工具.
/// </summary>
public sealed class ZhiPuWebSearchParameters
{
    /// <summary>
    /// 获取或设置是否启用.
    /// </summary>
    public bool? Enable { get; set; }

    /// <summary>
    /// 获取或设置搜索关键词.
    /// </summary>
    public string? SearchQuery { get; set; }

    /// <summary>
    /// 获取或设置搜索来源.
    /// </summary>
    public bool? SearchResult { get; set; }

    /// <summary>
    /// 获取或设置搜索提示.
    /// </summary>
    public string? SearchPrompt { get; set; }
}
