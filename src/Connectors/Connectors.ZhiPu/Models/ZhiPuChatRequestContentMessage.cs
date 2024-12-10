// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.ZhiPu.Models;

internal sealed class ZhiPuChatRequestContentMessage
{
    public required string Role { get; set; }

    public IList<ZhiPuChatContent>? Content { get; set; }
}

internal sealed class ZhiPuChatContent
{
    public required string Type { get; set; }

    public string? Text { get; set; }

    public ZhiPuContentUrl? ImageUrl { get; set; }

    public ZhiPuContentUrl? VideoUrl { get; set; }

    public static ZhiPuChatContent CreateTextMessage(string text)
    {
        return new ZhiPuChatContent
        {
            Type = "text",
            Text = text,
        };
    }

    public static ZhiPuChatContent CreateImageMessage(string imageUrl)
    {
        return new ZhiPuChatContent
        {
            Type = "image_url",
            ImageUrl = new ZhiPuContentUrl
            {
                Url = imageUrl,
            },
        };
    }

    public static ZhiPuChatContent CreateVideoMessage(string videoUrl)
    {
        return new ZhiPuChatContent
        {
            Type = "video_url",
            VideoUrl = new ZhiPuContentUrl
            {
                Url = videoUrl,
            },
        };
    }
}

internal sealed class ZhiPuContentUrl
{
    public string? Url { get; set; }
}