// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Ali.Models.Translation;

internal sealed class AliTranslateRequest
{
    public required string FormatType { get; set; }

    public required string SourceLanguage { get; set; }

    public required string TargetLanguage { get; set; }

    public required string SourceText { get; set; }

    public string Scene { get; set; } = "general";
}
