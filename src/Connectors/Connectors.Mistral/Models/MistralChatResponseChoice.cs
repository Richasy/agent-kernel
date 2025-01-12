// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Mistral.Models;

internal sealed class MistralChatResponseChoice
{
    public int Index { get; set; }

    public MistralChatMessage? Message { get; set; }

    public MistralChatMessage? Delta { get; set; }

    public required string FinishReason { get; set; }
}
