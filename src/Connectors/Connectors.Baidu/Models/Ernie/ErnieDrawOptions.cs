// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Baidu.Models.Ernie;

/// <summary>
/// 文心一言绘制选项.
/// </summary>
public sealed class ErnieDrawOptions : DrawOptions
{
    /// <summary>
    /// 获取或设置风格.
    /// </summary>
    public string? Style { get; set; }

    /// <summary>
    /// 水印.
    /// </summary>
    public string? TextContent { get; set; }
}
