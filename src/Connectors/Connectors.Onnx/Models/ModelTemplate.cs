// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Onnx.Models;

internal sealed class ModelTemplate
{
    public string? system { get; set; }

    public string? user { get; set; }

    public string? assistant { get; set; }

    public string? prompt { get; set; }
}
