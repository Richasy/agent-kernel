// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Volcano.Models.Audio;

internal sealed class VolcanoAudioRequest
{
    [JsonPropertyName("app")]
    public required AppData App { get; set; }

    [JsonPropertyName("user")]
    public UserData User { get; set; } = new();

    [JsonPropertyName("audio")]
    public required AudioData Audio { get; set; }

    [JsonPropertyName("request")]
    public required RequestData Request { get; set; }

    internal sealed class AppData
    {
        [JsonPropertyName("appid")]
        public required string AppId { get; set; }
        [JsonPropertyName("token")]
        public required string Token { get; set; }
        [JsonPropertyName("cluster")]
        public string Cluster { get; set; } = "volcano_tts";
    }

    internal sealed class UserData
    {
        [JsonPropertyName("uid")]
        public string UserId { get; set; } = "agent-kernel";
    }

    internal sealed class AudioData
    {
        [JsonPropertyName("voice_type")]
        public string? VoiceId { get; set; }

        [JsonPropertyName("rate")]
        public int? SampleRate { get; set; }

        [JsonPropertyName("encoding")]
        public string? Encoding { get; set; } = "wav";

        [JsonPropertyName("speed_ratio")]
        public double? Speed { get; set; }
    }

    internal sealed class RequestData
    {
        [JsonPropertyName("reqid")]
        public string RequestId { get; set; } = Guid.NewGuid().ToString("N");

        [JsonPropertyName("text")]
        public required string Text { get; set; }

        [JsonPropertyName("text_type")]
        public string? TextType { get; set; }

        [JsonPropertyName("operation")]
        public string Operation { get; set; } = "query";
    }
}
