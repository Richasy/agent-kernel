// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.ZhiPu.Models;

internal sealed class ZhiPuChatRequestAssistantMessage
{
    public required string Role { get; set; } = "assistant";

    public string? Content { get; set; }

    public IList<ZhiPuToolCall>? ToolCalls { get; set; }
}
