// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Azure.Models.Translation;

internal sealed class TranslationResponse
{
    public DetectedLanguageResponse? DetectedLanguage { get; set; }

    public IList<TranslationResponseChoice>? Translations { get; set; }
}
