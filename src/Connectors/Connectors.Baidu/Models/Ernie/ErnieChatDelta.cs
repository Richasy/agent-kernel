// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Baidu.Models;

internal sealed class ErnieChatDelta
{
    public string? Content { get; set; }

    public IList<ErnieToolCall>? ToolCalls { get; set; }
}
