// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Volcano.Models.Translation;

internal sealed class VolcanoTranslateResponse
{
    public IList<TranslationItem>? TranslationList { get; set; }

    internal sealed class TranslationItem
    {
        public string? Translation { get; set; }

        public string? DetectedSourceLanguage { get; set; }
    }
}
