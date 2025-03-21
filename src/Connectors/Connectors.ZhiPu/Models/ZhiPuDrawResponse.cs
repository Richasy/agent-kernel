// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.ZhiPu.Models;

internal sealed class ZhiPuDrawResponse
{
    [JsonPropertyName("created")]
    public long? Created { get; set; }

    [JsonPropertyName("data")]
    public List<ZhiPuDrawData>? Data { get; set; }

    [JsonPropertyName("content_filter")]
    public List<ZhiPuDrawContentFilter>? ContentFilter { get; set; }
}

internal sealed class ZhiPuDrawData
{
    [JsonPropertyName("url")]
    public string? Url { get; set; }
}

internal sealed class ZhiPuDrawContentFilter
{
    [JsonPropertyName("role")]
    public string? Role { get; set; }

    [JsonPropertyName("level")]
    public int? Level { get; set; }
}