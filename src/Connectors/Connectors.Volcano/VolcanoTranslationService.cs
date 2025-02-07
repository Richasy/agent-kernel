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
    private VolcanoTranslationServiceConfig? _config;

    /// <inheritdoc/>
    public TranslateServiceConfig? Config => _config;

    /// <inheritdoc/>
    public ITranslateClient? Client { get; set; }

    /// <inheritdoc/>
    public void Initialize(TranslateServiceConfig? config)
    {
        if (config is not VolcanoTranslationServiceConfig volcanoConfig)
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
            { "af", new("af") },
            { "sq", new("sq") },
            { "ar", new("ar") },
            { "hy", new("hy") },
            { "az", new("az") },
            { "eu", new("eu") },
            { "be", new("be") },
            { "bn", new("bn") },
            { "bg", new("bg") },
            { "my", new("my") },
            { "ca", new("ca") },
            { "zh", new("zh") },
            { "hr", new("hr") },
            { "cs", new("cs") },
            { "da", new("da") },
            { "nl", new("nl") },
            { "eo", new("eo") },
            { "et", new("et") },
            { "tl", new("tl") },
            { "fi", new("fi") },
            { "fr", new("fr") },
            { "gl", new("gl") },
            { "ka", new("ka") },
            { "de", new("de") },
            { "el", new("el") },
            { "ht", new("ht") },
            { "gu", new("gu") },
            { "ha", new("ha") },
            { "iw", new("iw") },
            { "he", new("he") },
            { "hi", new("hi") },
            { "hmn", new("hmn") },
            { "hu", new("hu") },
            { "is", new("is") },
            { "ig", new("ig") },
            { "id", new("id") },
            { "ga", new("ga") },
            { "it", new("it") },
            { "ja", new("ja") },
            { "kn", new("kn") },
            { "kk", new("kk") },
            { "km", new("km") },
            { "rw", new("rw") },
            { "rn", new("rn") },
            { "ko", new("ko") },
            { "ku", new("ku") },
            { "ky", new("ky") },
            { "lo", new("lo") },
            { "la", new("la") },
            { "lv", new("lv") },
            { "lt", new("lt") },
            { "lb", new("lb") },
            { "mk", new("mk") },
            { "mg", new("mg") },
            { "ms", new("ms") },
            { "ml", new("ml") },
            { "mt", new("mt") },
            { "mi", new("mi") },
            { "mr", new("mr") },
            { "mn", new("mn") },
            { "my", new("my") },
            { "ne", new("ne") },
            { "no", new("no") },
            { "ps", new("ps") },
            { "fa", new("fa") },
            { "pl", new("pl") },
            { "pt", new("pt") },
            { "pa", new("pa") },
            { "ro", new("ro") },
            { "ru", new("ru") },
            { "sm", new("sm") },
            { "gd", new("gd") },
            { "sr", new("sr") },
            { "st", new("st") },
            { "sn", new("sn") },
            { "sd", new("sd") },
            { "si", new("si") },
            { "sk", new("sk") },
            { "sl", new("sl") },
            { "so", new("so") },
            { "es", new("es") },
            { "su", new("su") },
            { "sw", new("sw") },
            { "sv", new("sv") },
            { "tg", new("tg") },
            { "ta", new("ta") },
            { "te", new("te") },
            { "th", new("th") },
            { "tr", new("tr") },
            { "uk", new("uk") },
            { "ur", new("ur") },
            { "uz", new("uz") },
            { "vi", new("vi") },
            { "cy", new("cy") },
            { "xho", new("xho") },
            { "yua", new("yua") },
            { "zu", new("zu") },
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
