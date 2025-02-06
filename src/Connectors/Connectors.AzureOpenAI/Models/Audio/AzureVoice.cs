// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Azure.Models.Audio;

internal sealed class AzureVoice
{
    [JsonPropertyName("Name")]
    public string? Name { get; set; }

    [JsonPropertyName("DisplayName")]
    public string? DisplayName { get; set; }

    [JsonPropertyName("LocalName")]
    public string? LocalName { get; set; }

    [JsonPropertyName("ShortName")]
    public string? ShortName { get; set; }

    [JsonPropertyName("Gender")]
    public string? Gender { get; set; }

    [JsonPropertyName("Locale")]
    public string? Locale { get; set; }

    [JsonPropertyName("LocaleName")]
    public string? LocaleName { get; set; }

    [JsonPropertyName("SampleRateHertz")]
    public string? SampleRateHertz { get; set; }

    [JsonPropertyName("VoiceType")]
    public string? VoiceType { get; set; }

    [JsonPropertyName("Status")]
    public string? Status { get; set; }
}
