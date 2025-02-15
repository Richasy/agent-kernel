// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Baidu.Models;

/// <summary>
/// 文心一言网络搜索工具.
/// </summary>
public sealed class ErnieWebSearchParameters
{
    /// <summary>
    /// 是否开启实时搜索功能.
    /// </summary>
    public bool? Enable { get; set; }

    /// <summary>
    /// 是否开启上角标返回.
    /// </summary>
    public bool? EnableCitation { get; set; }

    /// <summary>
    /// 是否返回搜索溯源信息.
    /// </summary>
    public bool? EnableTrace { get; set; }
}
