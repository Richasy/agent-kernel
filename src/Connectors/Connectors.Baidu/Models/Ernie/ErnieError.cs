// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Baidu.Models;

internal sealed class ErnieErrorResponse
{
    public ErnieError? Error { get; set; }

    public string? Id { get; set; }
}

internal sealed class ErnieError
{
    public string? Code { get; set; }

    public string? Message { get; set; }

    public string? Type { get; set; }
}
