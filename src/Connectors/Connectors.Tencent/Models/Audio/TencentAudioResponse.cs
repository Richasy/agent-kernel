// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Tencent.Models.Audio;

internal sealed class TencentAudioResponse
{
    public AudioResponseContent? Response { get; set; }

    internal sealed class AudioResponseContent
    {
        public string? Audio { get; set; }

        public string? SessionId { get; set; }

        public string? RequestId { get; set; }

        public TencentResponseError? Error { get; set; }
    }
}
