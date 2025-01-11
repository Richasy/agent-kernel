// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;
using System.Globalization;

namespace Richasy.AgentKernel.Connectors.Tencent.Core;

public sealed partial class TencentTranslateClient
{
    /// <inheritdoc/>
    public TranslationLanguages GetSupportedLanguages()
    {
        var languages = new Dictionary<string, CultureInfo?>
        {
            { "auto", null },
            { "ar", new("ar") },
            { "hi", new("hi") },
            { "id", new("id") },
            { "ms", new("ms") },
            { "th", new("th") },
            { "vi", new("vi") },
            { "pt", new("pt") },
            { "ru", new("ru") },
            { "tr", new("tr") },
            { "uk", new("uk") },
            { "en", new("en") },
            { "es", new("es") },
            { "it", new("it") },
            { "de", new("de") },
            { "fr", new("fr") },
            { "ja", new("ja") },
            { "ko", new("ko") },
            { "zh-TW", new("zh-Hant") },
            { "zh", new("zh-Hans") },
        };

        return new TranslationLanguages(languages, languages.Skip(1).ToDictionary());
    }
}
