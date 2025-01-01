// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Baidu.Models;

internal struct ErnieResponseFormat
{
    public string Type { get; set; }

    public static ErnieResponseFormat TextFormat => new() { Type = "text" };

    public static ErnieResponseFormat JsonFormat => new() { Type = "json_object" };
}
