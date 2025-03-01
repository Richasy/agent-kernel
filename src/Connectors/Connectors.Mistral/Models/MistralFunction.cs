// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Mistral.Models;

internal sealed class MistralFunction
{
    public required string Name { get; set; }

    public string? Description { get; set; }

    public MistralFunctionToolParameters? Parameters { get; set; }
}
