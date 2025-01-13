// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Baidu.Models.Ernie;

internal sealed class ErnieGetImageRequest
{
    [JsonPropertyName("taskId")]
    public string? BasicTaskId { get; set; }

    [JsonPropertyName("task_id")]
    public string? AdvancedTaskId { get; set; }
}
