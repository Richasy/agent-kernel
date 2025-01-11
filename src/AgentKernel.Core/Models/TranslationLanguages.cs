// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Globalization;
using Richasy.AgentKernel.Translation;

namespace Richasy.AgentKernel.Models;

/// <summary>
/// Represents metadata that describes a <see cref="ITranslateClient"/>.
/// </summary>
public sealed class TranslationLanguages(Dictionary<string, CultureInfo?> source, Dictionary<string, CultureInfo?> target)
{
    /// <summary>
    /// The source languages that the client supports.
    /// </summary>
    public Dictionary<string, CultureInfo?> SourceLanguages { get; } = source;

    /// <summary>
    /// The target languages that the client supports.
    /// </summary>
    public Dictionary<string, CultureInfo?> TargetLanguages { get; } = target;
}
