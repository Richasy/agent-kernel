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

// 定义 AOT 兼容的类型
internal sealed class EdgeSpeechConfig
{
    [JsonPropertyName("context")]
    public EdgeContext? Context { get; set; }
}

internal sealed class EdgeContext
{
    [JsonPropertyName("synthesis")]
    public EdgeSynthesis? Synthesis { get; set; }
}

internal sealed class EdgeSynthesis
{
    [JsonPropertyName("audio")]
    public EdgeAudio? Audio { get; set; }
}

internal sealed class EdgeAudio
{
    [JsonPropertyName("metadataoptions")]
    public EdgeMetadataOptions? MetadataOptions { get; set; }

    [JsonPropertyName("outputFormat")]
    public string? OutputFormat { get; set; }
}

internal sealed class EdgeMetadataOptions
{
    [JsonPropertyName("sentenceBoundaryEnabled")]
    public bool SentenceBoundaryEnabled { get; set; }

    [JsonPropertyName("wordBoundaryEnabled")]
    public bool WordBoundaryEnabled { get; set; }
}
