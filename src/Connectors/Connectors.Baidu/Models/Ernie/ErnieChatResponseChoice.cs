// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Baidu.Models;

internal sealed class ErnieChatResponseChoice
{
    public int Index { get; set; }

    public ErnieChatMessage? Message { get; set; }

    public ErnieChatDelta? Delta { get; set; }

    public string? FinishReason { get; set; }

    public int? Flag { get; set; }

    public int BanRound { get; set; }
}
