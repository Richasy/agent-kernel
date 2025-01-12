// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Mistral.Models;

internal sealed class MistralChatMessage
{
    public string? Role { get; set; }

    public string? Content { get; set; }

    public bool? Prefix { get; set; }

    public IList<MistralToolCall>? ToolCalls { get; set; }

    public string? ToolCallId { get; set; }

    public string? Name { get; set; }
}
