// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Tencent.Models.Translation;

/// <summary>
/// Tencent Translation Options.
/// </summary>
public sealed class TencentTranslateOptions : TranslateOptions
{
    /// <summary>
    /// Untranslated Text.
    /// </summary>
    public string? UntranslatedText { get; set; }
}
