// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Baidu.Models.Ernie;

internal sealed class ErnieAdvancedDrawRequest
{
    public required string Prompt { get; set; }

    public required int Width { get; set; }

    public required int Height { get; set; }

    public string? TextContent { get; set; }

    public int? TaskTimeOut { get; set; }
}
