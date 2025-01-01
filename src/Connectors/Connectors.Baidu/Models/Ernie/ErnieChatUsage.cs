// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Baidu.Models;

internal sealed class ErnieChatUsage
{
    public int PromptTokens { get; set; }

    public int CompletionTokens { get; set; }

    public int TotalTokens { get; set; }
}
