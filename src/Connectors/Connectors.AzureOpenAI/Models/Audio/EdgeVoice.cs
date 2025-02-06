// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Azure.Models.Audio;

internal sealed class EdgeVoice
{
    [JsonPropertyName("Name")]
    public string? Name { get; set; }

    [JsonPropertyName("ShortName")]
    public string? ShortName { get; set; }

    [JsonPropertyName("Gender")]
    public string? Gender { get; set; }

    [JsonPropertyName("Locale")]
    public string? Locale { get; set; }

    [JsonPropertyName("SuggestedCodec")]
    public string? SuggestedCodec { get; set; }

    [JsonPropertyName("FriendlyName")]
    public string? FriendlyName { get; set; }

    [JsonPropertyName("Status")]
    public string? Status { get; set; }

    [JsonPropertyName("VoiceTag")]
    public EdgeVoiceTag? VoiceTag { get; set; }
}

internal sealed class EdgeVoiceTag
{
    [JsonPropertyName("ContentCategories")]
    public string[]? ContentCategories { get; set; }

    [JsonPropertyName("VoicePersonalities")]
    public string[]? VoicePersonalities { get; set; }
}
