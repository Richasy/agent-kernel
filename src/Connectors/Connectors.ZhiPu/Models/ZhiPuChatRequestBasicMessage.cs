// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.ZhiPu.Models;

internal sealed class ZhiPuChatRequestBasicMessage
{
    public required string Role { get; set; }

    public string? Content { get; set; }
}