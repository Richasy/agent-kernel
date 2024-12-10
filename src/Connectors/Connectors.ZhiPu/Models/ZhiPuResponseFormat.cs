// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.ZhiPu.Models;

internal struct ZhiPuResponseFormat
{
    public string Type { get; set; }

    public static ZhiPuResponseFormat TextFormat => new() { Type = "text" };

    public static ZhiPuResponseFormat JsonFormat => new() { Type = "json_object" };
}
