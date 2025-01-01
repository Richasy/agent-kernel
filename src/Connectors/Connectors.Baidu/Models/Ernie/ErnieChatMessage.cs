// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Baidu.Models;

internal sealed class ErnieChatMessage
{
    public required string Role { get; set; }

    public required string Content { get; set; }

    public string? Name { get; set; }

    public IList<ErnieToolCall>? ToolCalls { get; set; }

    public string? ToolCallId { get; set; }
}
