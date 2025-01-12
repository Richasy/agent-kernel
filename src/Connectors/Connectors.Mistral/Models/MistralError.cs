// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Mistral.Models;

internal sealed class MistralErrorResponse
{
    public IList<MistralErrorDetail>? Detail { get; set; }
}

internal sealed class MistralErrorDetail
{
    public string[]? Loc { get; set; }

    public string? Msg { get; set; }

    public string? Type { get; set; }
}
