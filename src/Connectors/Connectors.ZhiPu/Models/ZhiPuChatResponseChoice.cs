// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.ZhiPu.Models;

internal sealed class ZhiPuChatResponseChoice
{
    public int Index { get; set; }

    public string? FinishReason { get; set; }

    public ZhiPuChatResponseMessage? Message { get; set; }

    public ZhiPuChatResponseMessage? Delta { get; set; }
}
