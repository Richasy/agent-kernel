// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Translation;

/// <summary>
/// Provides metadata about an <see cref="ITranslateClient"/>.
/// </summary>
public class TranslateClientMetadata(string? providerName, bool isTextSupported, bool isHtmlSupported)
{
    /// <summary>
    /// Gets the name of the translation provider.
    /// </summary>
    public string? ProviderName { get; } = providerName;

    /// <summary>
    /// Gets a value indicating whether the client supports text translation.
    /// </summary>
    public bool IsTextTranslateSupported { get; } = isTextSupported;

    /// <summary>
    /// Gets a value indicating whether the client supports HTML translation.
    /// </summary>
    public bool IsHtmlTranslateSupported { get; } = isHtmlSupported;
}
