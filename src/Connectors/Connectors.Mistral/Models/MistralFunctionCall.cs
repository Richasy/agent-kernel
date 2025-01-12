// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Mistral.Models;

internal sealed class MistralFunctionCall
{
    public required string Name { get; set; }

    [JsonConverter(typeof(BinaryJsonSchemaConverter))]
    public required BinaryData? Arguments { get; set; }
}
