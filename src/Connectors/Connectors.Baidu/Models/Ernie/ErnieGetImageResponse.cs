// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Baidu.Models.Ernie;

internal sealed class ErnieGetImageResponse
{
    public required ErnieGetImageData Data { get; set; }

    public long LogId { get; set; }
}

internal sealed class ErnieGetImageData
{
    public string? Style { get; set; }

    [JsonPropertyName("taskId")]
    public long TaskId { get; set; }

    [JsonPropertyName("imgUrls")]
    public IList<ErnieImageUrl>? ImgUrls { get; set; }

    public string? Text { get; set; }

    public int Status { get; set; }

    [JsonPropertyName("createTime")]
    public string? CreateTime { get; set; }

    public string? Img { get; set; }

    public string? Waiting { get; set; }
}

internal sealed class ErnieImageUrl
{
    public required string Image { get; set; }

    public string? ImgApproveConclusion { get; set; }
}