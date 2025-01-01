// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Baidu.Models;

internal sealed class ErnieFunction
{
    public required string Name { get; set; }

    public string? Description { get; set; }

    [JsonConverter(typeof(BinaryJsonSchemaConverter))]
    public BinaryData? Parameters { get; set; }
}