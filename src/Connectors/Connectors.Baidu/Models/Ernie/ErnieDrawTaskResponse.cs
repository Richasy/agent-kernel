// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Baidu.Models.Ernie;
internal sealed class ErnieDrawTaskResponse
{
    public ErnieDrawTaskData? Data { get; set; }

    public string? ErrorMsg { get; set; }

    public int? ErrorCode { get; set; }

    public long LogId { get; set; }
}

internal sealed class ErnieDrawTaskData
{
    [JsonPropertyName("taskId")]
    public long BasicTaskId { get; set; }

    [JsonPropertyName("task_id")]
    public string? AdvancedTaskId { get; set; }
}
