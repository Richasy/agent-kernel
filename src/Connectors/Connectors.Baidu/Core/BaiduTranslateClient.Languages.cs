// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;
using System.Globalization;

namespace Richasy.AgentKernel.Connectors.Baidu.Core;

public sealed partial class BaiduTranslateClient
{
    /// <inheritdoc/>
    public TranslationLanguages GetSupportedLanguages()
    {
        var languages = new Dictionary<string, CultureInfo?>
        {
            { "auto", null },
            { "zh", new("zh-Hans") },
            { "cht", new("zh-Hant") },
            { "yue", new("yue") },
            { "en", new("en") },
            { "jp", new("ja") },
            { "kor", new("ko") },
            { "fra", new("fr") },
            { "spa", new("es") },
            { "th", new("th") },
            { "ara", new("ar") },
            { "ru", new("ru") },
            { "pt", new("pt") },
            { "de", new("de") },
            { "it", new("it") },
            { "el", new("el") },
            { "nl", new("nl") },
            { "pl", new("pl") },
            { "bul", new("bg") },
            { "est", new("et") },
            { "dan", new("da") },
            { "fin", new("fi") },
            { "cs", new("cs") },
            { "rom", new("ro") },
            { "slo", new("sk") },
            { "swe", new("sv") },
            { "hu", new("hu") },
            { "vie", new("vi") },
        };

        return new TranslationLanguages(languages, languages.Skip(1).ToDictionary());
    }
}
