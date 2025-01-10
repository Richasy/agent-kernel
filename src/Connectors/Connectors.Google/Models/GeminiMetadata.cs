// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Collections.ObjectModel;

namespace Richasy.AgentKernel.Connectors.Google.Models;

/// <summary>
/// Represents the metadata associated with a Gemini response.
/// </summary>
internal sealed class GeminiMetadata : ReadOnlyDictionary<string, object?>
{
    internal GeminiMetadata() : base(new Dictionary<string, object?>()) { }

    private GeminiMetadata(IDictionary<string, object?> dictionary) : base(dictionary) { }

    /// <summary>
    /// Reason why the processing was finished.
    /// </summary>
    public GeminiFinishReason? FinishReason
    {
        get => GetValueFromDictionary(nameof(FinishReason)) as GeminiFinishReason?;
        internal init => SetValueInDictionary(value, nameof(FinishReason));
    }

    /// <summary>
    /// Index of the response.
    /// </summary>
    public int Index
    {
        get => (GetValueFromDictionary(nameof(Index)) as int?) ?? 0;
        internal init => SetValueInDictionary(value, nameof(Index));
    }

    /// <summary>
    /// The count of tokens in the prompt.
    /// </summary>
    public int PromptTokenCount
    {
        get => (GetValueFromDictionary(nameof(PromptTokenCount)) as int?) ?? 0;
        internal init => SetValueInDictionary(value, nameof(PromptTokenCount));
    }

    /// <summary>
    /// The count of token in the current candidate.
    /// </summary>
    public int CurrentCandidateTokenCount
    {
        get => (GetValueFromDictionary(nameof(CurrentCandidateTokenCount)) as int?) ?? 0;
        internal init => SetValueInDictionary(value, nameof(CurrentCandidateTokenCount));
    }

    /// <summary>
    /// The total count of tokens of the all candidate responses.
    /// </summary>
    public int CandidatesTokenCount
    {
        get => (GetValueFromDictionary(nameof(CandidatesTokenCount)) as int?) ?? 0;
        internal init => SetValueInDictionary(value, nameof(CandidatesTokenCount));
    }

    /// <summary>
    /// The total count of tokens (prompt + total candidates token count).
    /// </summary>
    public int TotalTokenCount
    {
        get => (GetValueFromDictionary(nameof(TotalTokenCount)) as int?) ?? 0;
        internal init => SetValueInDictionary(value, nameof(TotalTokenCount));
    }

    /// <summary>
    /// The reason why prompt was blocked.
    /// </summary>
    public string? PromptFeedbackBlockReason
    {
        get => GetValueFromDictionary(nameof(PromptFeedbackBlockReason)) as string;
        internal init => SetValueInDictionary(value, nameof(PromptFeedbackBlockReason));
    }

    /// <summary>
    /// List of safety ratings for the prompt feedback.
    /// </summary>
    public IReadOnlyList<GeminiSafetyRating>? PromptFeedbackSafetyRatings
    {
        get => GetValueFromDictionary(nameof(PromptFeedbackSafetyRatings)) as IReadOnlyList<GeminiSafetyRating>;
        internal init => SetValueInDictionary(value, nameof(PromptFeedbackSafetyRatings));
    }

    /// <summary>
    /// List of safety ratings for the response.
    /// </summary>
    public IReadOnlyList<GeminiSafetyRating>? ResponseSafetyRatings
    {
        get => GetValueFromDictionary(nameof(ResponseSafetyRatings)) as IReadOnlyList<GeminiSafetyRating>;
        internal init => SetValueInDictionary(value, nameof(ResponseSafetyRatings));
    }

    /// <summary>
    /// Converts a dictionary to a <see cref="GeminiMetadata"/> object.
    /// </summary>
    public static GeminiMetadata FromDictionary(IReadOnlyDictionary<string, object?> dictionary) => dictionary switch
    {
        null => throw new ArgumentNullException(nameof(dictionary)),
        GeminiMetadata metadata => metadata,
        IDictionary<string, object?> metadata => new GeminiMetadata(metadata),
        _ => new GeminiMetadata(dictionary.ToDictionary(pair => pair.Key, pair => pair.Value))
    };

    private void SetValueInDictionary(object? value, string propertyName)
        => Dictionary[propertyName] = value;

    private object? GetValueFromDictionary(string propertyName)
        => Dictionary.TryGetValue(propertyName, out var value) ? value : null;
}
