// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Tencent.Models.Draw;

internal sealed class HunyuanDrawCreateRequest
{
    public string? Prompt { get; set; }

    public string? Resolution { get; set; }

    public int LogoAdd { get; set; }

    public int? Revise { get; set; }

    public string? RspImgType { get; set; }
}
