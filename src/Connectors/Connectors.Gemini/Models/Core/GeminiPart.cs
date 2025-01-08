// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;
using System.Text.Json;

namespace Richasy.AgentKernel.Connectors.Google.Models.Core;

/// <summary>
/// Union field data can be only one of properties in class GeminiPart
/// </summary>
internal sealed class GeminiPart : IJsonOnDeserialized
{
    /// <summary>
    /// Gets or sets the text data.
    /// </summary>
    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the image or video as binary data.
    /// </summary>
    [JsonPropertyName("inlineData")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public InlineDataPart? InlineData { get; set; }

    /// <summary>
    /// Gets or sets the image or video as file uri.
    /// </summary>
    [JsonPropertyName("fileData")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public FileDataPart? FileData { get; set; }

    /// <summary>
    /// Checks whether only one property of the GeminiPart instance is not null.
    /// Returns true if only one property among Text, InlineData, FileData, FunctionCall, and FunctionResponse is not null,
    /// Otherwise, it returns false.
    /// </summary>
    public bool IsValid()
    {
        return (Text is not null ? 1 : 0) +
            (InlineData is not null ? 1 : 0) +
            (FileData is not null ? 1 : 0) == 1;
    }

    /// <inheritdoc />
    public void OnDeserialized()
    {
        if (!IsValid())
        {
            throw new JsonException(
                "GeminiPart is invalid. One and only one property among Text, InlineData, FileData, FunctionCall, and FunctionResponse should be set.");
        }
    }

    /// <summary>
    /// Inline media bytes like image or video data.
    /// </summary>
    internal sealed class InlineDataPart
    {
        /// <summary>
        /// The IANA standard MIME type of the source data.
        /// </summary>
        /// <remarks>
        /// Acceptable values include: "image/png", "image/jpeg", "image/heic", "image/heif", "image/webp".
        /// </remarks>
        [JsonPropertyName("mimeType")]
        [JsonRequired]
        public string MimeType { get; set; } = null!;

        /// <summary>
        /// Base64 encoded data
        /// </summary>
        [JsonPropertyName("data")]
        [JsonRequired]
        public string InlineData { get; set; } = null!;
    }

    /// <summary>
    /// File media bytes like image or video data.
    /// </summary>
    internal sealed class FileDataPart
    {
        /// <summary>
        /// The IANA standard MIME type of the source data.
        /// </summary>
        /// <remarks>
        /// Acceptable values include: "image/png", "image/jpeg", "video/mov", "video/mpeg", "video/mp4", "video/mpg", "video/avi", "video/wmv", "video/mpegps", "video/flv".
        /// </remarks>
        [JsonPropertyName("mimeType")]
        [JsonRequired]
        public string MimeType { get; set; } = null!;

        /// <summary>
        /// The Cloud Storage URI of the image or video to include in the prompt.
        /// The bucket that stores the file must be in the same Google Cloud project that's sending the request.
        /// </summary>
        [JsonPropertyName("fileUri")]
        [JsonRequired]
        public Uri FileUri { get; set; } = null!;
    }
}