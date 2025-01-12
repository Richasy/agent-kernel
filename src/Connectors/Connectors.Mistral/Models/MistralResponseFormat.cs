// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Mistral.Models;

internal struct MistralResponseFormat
{
    public string Type { get; set; }

    public static MistralResponseFormat TextFormat => new() { Type = "text" };

    public static MistralResponseFormat JsonFormat => new() { Type = "json_object" };
}
