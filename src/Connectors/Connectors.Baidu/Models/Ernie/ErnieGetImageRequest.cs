// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Baidu.Models.Ernie;

internal sealed class ErnieGetImageRequest
{
    [JsonPropertyName("taskId")]
    public required string TaskId { get; set; }
}
