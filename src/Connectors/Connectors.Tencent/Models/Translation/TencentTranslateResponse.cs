// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Tencent.Models.Translation;

internal sealed class TencentTranslateResponse
{
    public TextTranslateResponseContent? Response { get; set; }

    internal sealed class TextTranslateResponseContent
    {
        public string? TargetText { get; set; }

        public string? Source { get; set; }

        public string? Target { get; set; }

        public string? RequestId { get; set; }

        public int UsedAmount { get; set; }

        public TencentResponseError? Error { get; set; }
    }
}
