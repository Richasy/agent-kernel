// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Azure.Models.Translation;

internal sealed class DetectedLanguageResponse
{
    public string? Language { get; set; }

    public double Score { get; set; }
}
