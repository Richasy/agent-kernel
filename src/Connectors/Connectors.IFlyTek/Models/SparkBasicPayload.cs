// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.IFlyTek.Models;

internal sealed class SparkBasicRequestPayload
{
    [JsonPropertyName("message")]
    public SparkMessage? Message { get; set; }
}
