// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Volcano.Models.Translation;

internal sealed class VolcanoTranslateRequest
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SourceLanguage { get; set; }

    public string? TargetLanguage { get; set; }

    public IList<string>? TextList { get; set; }
}
