// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Baidu.Models.Ernie;

internal sealed class ErnieDrawRequest
{
    public required string Text { get; set; }

    public required string Resolution { get; set; }

    public string? Style { get; set; }

    public int? Num { get; set; }

    public string? TextContent { get; set; }
}
