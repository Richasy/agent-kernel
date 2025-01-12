// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Mistral.Models;

internal sealed class MistralTool
{
    public string Type { get; set; } = "function";

    public required MistralFunction Function { get; set; }
}
