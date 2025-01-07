// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Tencent.Models.Translation;

internal sealed class TencentTranslateRequest
{
    public string? SourceText { get; set; }

    public string? Source { get; set; }

    public string? Target { get; set; }

    public int ProjectId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? UntranslatedText { get; set; }
}
