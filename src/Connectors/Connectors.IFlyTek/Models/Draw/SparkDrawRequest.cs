// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.IFlyTek.Models.Draw;

internal sealed class SparkDrawRequest
{
    [JsonPropertyName("header")]
    public SparkRequestHeader? Header { get; set; }

    [JsonPropertyName("parameter")]
    public SparkDrawRequestParametersContainer? Parameter { get; set; }

    [JsonPropertyName("payload")]
    public SparkBasicRequestPayload? Payload { get; set; }

    internal sealed class SparkDrawRequestParametersContainer
    {
        [JsonPropertyName("chat")]
        public SparkDrawRequestParameters? Image { get; set; }
    }

    internal sealed class SparkDrawRequestParameters
    {
        [JsonPropertyName("domain")]
        public string Domain { get; set; } = "general";

        [JsonPropertyName("width")]
        public int Width { get; set; } = 512;

        [JsonPropertyName("height")]
        public int Height { get; set; } = 512;
    }
}