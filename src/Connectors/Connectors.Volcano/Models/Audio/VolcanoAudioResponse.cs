// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Volcano.Models.Audio;

internal sealed class VolcanoAudioResponse
{
    [JsonPropertyName("reqid")]
    public string? RequestId { get; set; }

    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("sequence")]
    public int? Sequence { get; set; }

    [JsonPropertyName("data")]
    public string? Data { get; set; }
}
