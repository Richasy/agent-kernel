// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.ZhiPu.Models;

internal sealed class ZhiPuErrorResponse
{
    public ZhiPuError? Error { get; set; }
}

internal sealed class ZhiPuError
{
    public string? Code { get; set; }

    public string? Message { get; set; }
}
