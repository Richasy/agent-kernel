// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Models;

/// <summary>
/// Represents the completion of translation.
/// </summary>
public sealed class TranslateCompletion
{
    /// <summary>
    /// Gets or sets the source content that needs to be translated.
    /// </summary>
    public string? SourceContent { get; set; }

    /// <summary>
    /// Gets or sets the result of translation.
    /// </summary>
    public required string Result { get; set; }

    /// <summary>
    /// Gets or sets the source language of the content.
    /// </summary>
    public string? SourceLanguage { get; set; }

    /// <summary>
    /// Gets or sets the target language of the content.
    /// </summary>
    public string? TargetLanguage { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the completion.
    /// </summary>
    public string? Id { get; set; }

    /// <inheritdoc/>
    public override string ToString()
        => $"SourceContent: {SourceContent}\nResult: {Result}\nSourceLanguage: {SourceLanguage}\nTargetLanguage: {TargetLanguage}";
}
