// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Youdao.Core;
using Richasy.AgentKernel.Connectors.Youdao.Models;
using Richasy.AgentKernel.Models;
using Richasy.AgentKernel.Translation;
using RichasyKernel;
using System.Globalization;

namespace Richasy.AgentKernel.Connectors.Youdao;

/// <summary>
/// Youdao translation service.
/// </summary>
public sealed class YoudaoTranslationService : ITranslationService
{
    private YoudaoTranslationServiceConfig? _config;

    /// <inheritdoc/>
    public TranslationServiceConfig? Config => _config;

    /// <inheritdoc/>
    public ITranslateClient? Client { get; set; }

    /// <inheritdoc/>
    public void Initialize(TranslationServiceConfig? config)
    {
        if (config is not YoudaoTranslationServiceConfig youdaoConfig)
        {
            throw new KernelException("Configuration is invalid");
        }

        if (_config != null && youdaoConfig.Equals(_config))
        {
            return;
        }

        _config = youdaoConfig;
        Client?.Dispose();
        Client = new YoudaoTranslateClient(youdaoConfig);
    }

    /// <inheritdoc/>
    public TranslationLanguages GetSupportedLanguages()
    {
        var languages = new Dictionary<string, CultureInfo?>
        {
            { "auto", null },
            { "ar", new("ar") },
            { "de", new("de") },
            { "en", new("en") },
            { "es", new("es") },
            { "fr", new("fr") },
            { "hi", new("hi") },
            { "id", new("id") },
            { "it", new("it") },
            { "ja", new("ja") },
            { "ko", new("ko") },
            { "nl", new("nl") },
            { "pt", new("pt") },
            { "ru", new("ru") },
            { "th", new("th") },
            { "vi", new("vi") },
            { "zh-CHS", new("zh-CHS") },
            { "zh-CHT", new("zh-CHT") },
            { "af", new("af") },
            { "am", new("am") },
            { "az", new("az") },
            { "be", new("be") },
            { "bg", new("bg") },
            { "bn", new("bn") },
            { "bs", new("bs") },
            { "ca", new("ca") },
            { "ceb", new("ceb") },
            { "co", new("co") },
            { "cs", new("cs") },
            { "cy", new("cy") },
            { "da", new("da") },
            { "el", new("el") },
            { "eo", new("eo") },
            { "et", new("et") },
            { "eu", new("eu") },
            { "fa", new("fa") },
            { "fi", new("fi") },
            { "fj", new("fj") },
            { "ga", new("ga") },
            { "gd", new("gd") },
            { "gl", new("gl") },
            { "gu", new("gu") },
            { "ha", new("ha") },
            { "haw", new("haw") },
            { "he", new("he") },
            { "hr", new("hr") },
            { "ht", new("ht") },
            { "hu", new("hu") },
            { "hy", new("hy") },
            { "ig", new("ig") },
            { "is", new("is") },
            { "jw", new("jw") },
            { "ka", new("ka") },
            { "kk", new("kk") },
            { "km", new("km") },
            { "kn", new("kn") },
            { "ku", new("ku") },
            { "ky", new("ky") },
            { "la", new("la") },
            { "lb", new("lb") },
            { "lo", new("lo") },
            { "lt", new("lt") },
            { "lv", new("lv") },
            { "mg", new("mg") },
            { "mi", new("mi") },
            { "mk", new("mk") },
            { "ml", new("ml") },
            { "mn", new("mn") },
            { "mr", new("mr") },
            { "ms", new("ms") },
            { "mt", new("mt") },
            { "my", new("my") },
            { "ne", new("ne") },
            { "no", new("no") },
            { "nl", new("nl") },
            { "ny", new("ny") },
            { "pa", new("pa") },
            { "pl", new("pl") },
            { "ps", new("ps") },
            { "ro", new("ro") },
            { "sd", new("sd") },
            { "si", new("si") },
            { "sk", new("sk") },
            { "sl", new("sl") },
            { "sm", new("sm") },
            { "sn", new("sn") },
            { "so", new("so") },
            { "sq", new("sq") },
            { "sr-Cyrl", new("sr-Cyrl") },
            { "sr-Latn", new("sr-Latn") },
            { "st", new("st") },
            { "su", new("su") },
            { "sv", new("sv") },
            { "sw", new("sw") },
            { "ta", new("ta") },
            { "te", new("te") },
            { "tg", new("tg") },
            { "tl", new("tl") },
            { "tlh", new("tlh") },
            { "to", new("to") },
            { "tr", new("tr") },
            { "ty", new("ty") },
            { "uk", new("uk") },
            { "ur", new("ur") },
            { "uz", new("uz") },
            { "xh", new("xh") },
            { "yi", new("yi") },
            { "yo", new("yo") },
            { "yue", new("yue") },
            { "zu", new("zu") },
        };

        return new TranslationLanguages(languages, languages.Skip(1).ToDictionary());
    }
}
