// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Tencent.Models.Draw;

internal sealed class HunyuanDrawCreateResponse
{
    public HunyuanDrawCreateData? Response { get; set; }
}

internal sealed class HunyuanDrawCreateData
{
    public string? JobId { get; set; }

    public string? RequestId { get; set; }
}
