// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Models;

/// <summary>
/// Represents the options for a translation request.
/// </summary>
public class TranslateOptions
{
    /// <summary>
    /// Gets or sets the source content that needs to be translated.
    /// </summary>
    public string? SourceLanguage { get; set; }

    /// <summary>
    /// Gets or sets the target language of the content.
    /// </summary>
    public string? TargetLanguage { get; set; }
}
