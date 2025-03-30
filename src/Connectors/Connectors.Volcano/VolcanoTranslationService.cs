// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Volcano.Core;
using Richasy.AgentKernel.Connectors.Volcano.Models;
using Richasy.AgentKernel.Models;
using Richasy.AgentKernel.Translation;
using RichasyKernel;
using System.Globalization;

namespace Richasy.AgentKernel.Connectors.Volcano;

/// <summary>
/// Translation service that uses the Volcano translation service.
/// </summary>
public sealed class VolcanoTranslationService : ITranslateService
{
    private VolcanoTranslateServiceConfig? _config;

    /// <inheritdoc/>
    public TranslateServiceConfig? Config => _config;

    /// <inheritdoc/>
    public ITranslateClient? Client { get; set; }

    /// <inheritdoc/>
    public void Initialize(TranslateServiceConfig? config)
    {
        if (config is not VolcanoTranslateServiceConfig volcanoConfig)
        {
            throw new KernelException("Configuration is invalid");
        }

        if (_config != null && volcanoConfig.Equals(_config))
        {
            return;
        }

        _config = volcanoConfig;
        Client?.Dispose();
        Client = new VolcanoTranslateClient(volcanoConfig);
    }

    /// <inheritdoc/>
    public TranslationLanguages GetSupportedLanguages()
    {
        var languages = new Dictionary<string, CultureInfo?>
        {
            { "auto", null },
            { "zh-Hant", new("zh-Hant") },
            { "zh-Hant-hk", new("zh-Hant-hk") },
            { "zh-Hant-tw", new("zh-Hant-tw") },
            { "tn", new("tn") },
            { "vi", new("vi") },
            { "iu", new("iu") },
            { "it", new("it") },
            { "id", new("id") },
            { "hi", new("hi") },
            { "en", new("en") },
            { "ho", new("ho") },
            { "he", new("he") },
            { "es", new("es") },
            { "el", new("el") },
            { "uk", new("uk") },
            { "ur", new("ur") },
            { "tk", new("tk") },
            { "tr", new("tr") },
            { "ti", new("ti") },
            { "ty", new("ty") },
            { "tl", new("tl") },
            { "to", new("to") },
            { "th", new("th") },
            { "ta", new("ta") },
            { "te", new("te") },
            { "sl", new("sl") },
            { "sk", new("sk") },
            { "ss", new("ss") },
            { "eo", new("eo") },
            { "sm", new("sm") },
            { "sg", new("sg") },
            { "st", new("st") },
            { "sv", new("sv") },
            { "ja", new("ja") },
            { "tw", new("tw") },
            { "qu", new("qu") },
            { "pt", new("pt") },
            { "pa", new("pa") },
            { "no", new("no") },
            { "nb", new("nb") },
            { "nr", new("nr") },
            { "my", new("my") },
            { "bn", new("bn") },
            { "mn", new("mn") },
            { "mh", new("mh") },
            { "mk", new("mk") },
            { "ml", new("ml") },
            { "mr", new("mr") },
            { "ms", new("ms") },
            { "lu", new("lu") },
            { "ro", new("ro") },
            { "lt", new("lt") },
            { "lv", new("lv") },
            { "lo", new("lo") },
            { "kj", new("kj") },
            { "hr", new("hr") },
            { "kn", new("kn") },
            { "ki", new("ki") },
            { "cs", new("cs") },
            { "ca", new("ca") },
            { "nl", new("nl") },
            { "ko", new("ko") },
            { "ht", new("ht") },
            { "gu", new("gu") },
            { "ka", new("ka") },
            { "kl", new("kl") },
            { "km", new("km") },
            { "lg", new("lg") },
            { "kg", new("kg") },
            { "fi", new("fi") },
            { "fj", new("fj") },
            { "fr", new("fr") },
            { "ru", new("ru") },
            { "ng", new("ng") },
            { "de", new("de") },
            { "tt", new("tt") },
            { "da", new("da") },
            { "ts", new("ts") },
            { "cv", new("cv") },
            { "fa", new("fa") },
            { "bs", new("bs") },
            { "pl", new("pl") },
            { "bi", new("bi") },
            { "nd", new("nd") },
            { "ba", new("ba") },
            { "bg", new("bg") },
            { "az", new("az") },
            { "ar", new("ar") },
            { "af", new("af") },
            { "sq", new("sq") },
            { "ab", new("ab") },
            { "os", new("os") },
            { "ee", new("ee") },
            { "et", new("et") },
            { "ay", new("ay") },
            { "lzh", new("lzh") },
            { "am", new("am") },
            { "ckb", new("ckb") },
            { "cy", new("cy") },
            { "gl", new("gl") },
            { "ha", new("ha") },
            { "hy", new("hy") },
            { "ig", new("ig") },
            { "kmr", new("kmr") },
            { "ln", new("ln") },
            { "nso", new("nso") },
            { "ny", new("ny") },
            { "om", new("om") },
            { "sn", new("sn") },
            { "so", new("so") },
            { "sr", new("sr") },
            { "sw", new("sw") },
            { "xh", new("xh") },
            { "yo", new("yo") },
            { "zu", new("zu") },
            { "bo", new("bo") },
            { "nan", new("nan") },
            { "wuu", new("wuu") },
            { "yue", new("yue") },
            { "cmn", new("cmn") },
            { "ug", new("ug") },
            { "fuv", new("fuv") },
            { "hu", new("hu") },
            { "kam", new("kam") },
            { "luo", new("luo") },
            { "rw", new("rw") },
            { "umb", new("umb") },
            { "wo", new("wo") },
        };

        return new TranslationLanguages(languages, languages.Skip(1).ToDictionary());
    }
}
