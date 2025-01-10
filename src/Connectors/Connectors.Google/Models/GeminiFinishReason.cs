// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;
using System.Text.Json;

namespace Richasy.AgentKernel.Connectors.Google.Models;

/// <summary>
/// Represents a Gemini Finish Reason.
/// </summary>
[JsonConverter(typeof(GeminiFinishReasonConverter))]
internal readonly struct GeminiFinishReason : IEquatable<GeminiFinishReason>
{
    /// <summary>
    /// Default value. This value is unused.
    /// </summary>
    public static GeminiFinishReason Unspecified { get; } = new("FINISH_REASON_UNSPECIFIED");

    /// <summary>
    /// Natural stop point of the model or provided stop sequence.
    /// </summary>
    public static GeminiFinishReason Stop { get; } = new("STOP");

    /// <summary>
    /// The maximum number of tokens as specified in the request was reached.
    /// </summary>
    public static GeminiFinishReason MaxTokens { get; } = new("MAX_TOKENS");

    /// <summary>
    /// The candidate content was flagged for safety reasons.
    /// </summary>
    public static GeminiFinishReason Safety { get; } = new("SAFETY");

    /// <summary>
    /// The candidate content was flagged for recitation reasons.
    /// </summary>
    public static GeminiFinishReason Recitation { get; } = new("RECITATION");

    /// <summary>
    /// Unknown reason.
    /// </summary>
    public static GeminiFinishReason Other { get; } = new("OTHER");

    /// <summary>
    /// Gets the label of the property.
    /// Label is used for serialization.
    /// </summary>
    public string Label { get; }

    /// <summary>
    /// Represents a Gemini Finish Reason.
    /// </summary>
    [JsonConstructor]
    public GeminiFinishReason(string label)
    {
        if (string.IsNullOrWhiteSpace(label))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(label));
        }

        Label = label;
    }

    /// <summary>
    /// Represents the equality operator for comparing two instances of <see cref="GeminiFinishReason"/>.
    /// </summary>
    /// <param name="left">The left <see cref="GeminiFinishReason"/> instance to compare.</param>
    /// <param name="right">The right <see cref="GeminiFinishReason"/> instance to compare.</param>
    /// <returns><c>true</c> if the two instances are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(GeminiFinishReason left, GeminiFinishReason right)
        => left.Equals(right);

    /// <summary>
    /// Represents the inequality operator for comparing two instances of <see cref="GeminiFinishReason"/>.
    /// </summary>
    /// <param name="left">The left <see cref="GeminiFinishReason"/> instance to compare.</param>
    /// <param name="right">The right <see cref="GeminiFinishReason"/> instance to compare.</param>
    /// <returns><c>true</c> if the two instances are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(GeminiFinishReason left, GeminiFinishReason right)
        => !(left == right);

    /// <inheritdoc />
    public bool Equals(GeminiFinishReason other)
        => string.Equals(Label, other.Label, StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc />
    public override bool Equals(object? obj)
        => obj is GeminiFinishReason other && this == other;

    /// <inheritdoc />
    public override int GetHashCode()
        => StringComparer.OrdinalIgnoreCase.GetHashCode(Label ?? string.Empty);

    /// <inheritdoc />
    public override string ToString() => Label ?? string.Empty;
}

internal sealed class GeminiFinishReasonConverter : JsonConverter<GeminiFinishReason>
{
    public override GeminiFinishReason Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => new(reader.GetString()!);

    public override void Write(Utf8JsonWriter writer, GeminiFinishReason value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.Label);
}
