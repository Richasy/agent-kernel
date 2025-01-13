// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Tencent.Models.Draw;

internal sealed class HunyuanDrawLiteResponse
{
    public HunyuanDrawLiteData? Response { get; set; }

    internal sealed class HunyuanDrawLiteData
    {
        public string? ResultImage { get; set; }

        public string? RequestId { get; set; }
    }
}
