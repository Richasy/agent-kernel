// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Tencent.Models.Draw;

internal sealed class HunyuanDrawQueryResponse
{
    public HunyuanDrawQueryResponseContent? Response { get; set; }

    internal sealed class HunyuanDrawQueryResponseContent
    {
        public string? JobStatusCode { get; set; }

        public string? JobStatusMsg { get; set; }

        public string? JobErrorCode { get; set; }

        public string? JobErrorMsg { get; set; }

        public IList<string>? ResultImage { get; set; }

        public IList<string>? RevisedPrompt { get; set; }

        public string? RequestId { get; set; }
    }
}
