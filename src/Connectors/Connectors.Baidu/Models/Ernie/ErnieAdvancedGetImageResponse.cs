// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Baidu.Models.Ernie;

internal sealed class ErnieAdvancedGetImageResponse
{
    public required ErnieAdvancedGetImageData Data { get; set; }

    public long LogId { get; set; }
}

internal sealed class ErnieAdvancedGetImageData
{
    public int? TaskProgress { get; set; }

    public string? TaskStatus { get; set; }

    public long TaskId { get; set; }

    public IList<ErnieAdvancedSubTaskResult>? SubTaskResultList { get; set; }
}

internal sealed class ErnieAdvancedSubTaskResult
{
    public int SubTaskErrorCode { get; set; }

    public string? SubTaskStatus { get; set; }

    public int SubTaskProgress { get; set; }

    public IList<ErnieAdvancedSubTaskImage>? FinalImageList { get; set; }
}

internal sealed class ErnieAdvancedSubTaskImage
{
    public required string ImgUrl { get; set; }

    public string? ImgApproveConclusion { get; set; }

    public int Height { get; set; }

    public int Width { get; set; }
}