// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.ZhiPu.Models;

internal sealed class ZhiPuChatRequestToolMessage
{
    public required string Role { get; set; }

    public required string Content { get; set; }

    public required string ToolCallId { get; set; }
}
