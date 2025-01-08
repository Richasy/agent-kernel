// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Youdao.Models.Translation;

internal sealed class YoudaoTranslateResult
{
    [JsonPropertyName("errorCode")]
    public string? ErrorCode { get; set; }

    [JsonPropertyName("query")]
    public string? Query { get; set; }

    [JsonPropertyName("translation")]
    public string[]? Translation { get; set; }

    [JsonPropertyName("l")]
    public string? Language { get; set; }
}
