// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Mistral.Models;

internal sealed class MistralChatResponse
{
    public required string Id { get; set; }

    public required string Object { get; set; }

    public required string Model { get; set; }

    public IList<MistralChatResponseChoice>? Choices { get; set; }

    public long Created { get; set; }

    public MistralChatUsage? Usage { get; set; }
}
