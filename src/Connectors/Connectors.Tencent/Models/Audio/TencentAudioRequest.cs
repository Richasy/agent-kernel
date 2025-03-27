// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Tencent.Models.Audio;

internal sealed class TencentAudioRequest
{
    public string? Text { get; set; }

    public string SessionId { get; set; } = Guid.NewGuid().ToString("N");

    public double? Speed { get; set; }

    public int? VoiceType { get; set; }
}
