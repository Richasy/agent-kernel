// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Models;

/// <summary>
/// Represents audio content.
/// </summary>
public class VideoContent : DataContent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VideoContent"/> class.
    /// </summary>
    /// <param name="uri">The URI of the content. This can be a data URI.</param>
    /// <param name="mediaType">The media type (also known as MIME type) represented by the content.</param>
    public VideoContent(Uri uri, string? mediaType = null)
        : base(uri, mediaType)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VideoContent"/> class.
    /// </summary>
    /// <param name="uri">The URI of the content. This can be a data URI.</param>
    /// <param name="mediaType">The media type (also known as MIME type) represented by the content.</param>
    [JsonConstructor]
    public VideoContent([StringSyntax(StringSyntaxAttribute.Uri)] string uri, string? mediaType = null)
        : base(uri, mediaType)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VideoContent"/> class.
    /// </summary>
    /// <param name="data">The byte contents.</param>
    /// <param name="mediaType">The media type (also known as MIME type) represented by the content.</param>
    public VideoContent(ReadOnlyMemory<byte> data, string mediaType)
        : base(data, mediaType)
    {
    }
}
