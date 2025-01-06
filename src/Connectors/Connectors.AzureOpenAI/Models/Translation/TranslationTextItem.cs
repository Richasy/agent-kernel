// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Azure.Models.Translation;

internal sealed class TranslationTextItem(string text)
{
    public string Text { get; set; } = text;
}
