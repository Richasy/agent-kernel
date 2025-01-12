// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Mistral.Models;

internal sealed class MistralToolCall
{
    public string? Id { get; set; }

    public string Type { get; set; } = "function";

    public required MistralFunctionCall Function { get; set; }
}
