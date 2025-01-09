// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Ali.Models.Translation;

internal sealed class AliTranslateResponse
{
    public required string Code { get; set; }

    public string? Message { get; set; }

    public string? RequestId { get; set; }

    public AliTranslateResult? Data { get; set; }
}

internal sealed class AliTranslateResult
{
    public string? Translated { get; set; }

    public string? WordCount { get; set; }

    public string? DetectedLangauge { get; set; }
}
