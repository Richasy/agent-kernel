// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.ZhiPu.Models;

internal sealed class ZhiPuChatResponse
{
    public required string Id { get; set; }

    public required long Created { get; set; }

    public required string Model { get; set; }

    public IList<ZhiPuChatResponseChoice>? Choices { get; set; }

    public ZhiPuChatUsage? Usage { get; set; }

    public IList<ZhiPuWebSearchItem>? WebSearch { get; set; }
}
