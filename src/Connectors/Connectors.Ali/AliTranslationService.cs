// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Ali.Core;
using Richasy.AgentKernel.Connectors.Ali.Models;
using Richasy.AgentKernel.Models;
using Richasy.AgentKernel.Translation;
using RichasyKernel;
using System.Globalization;

namespace Richasy.AgentKernel.Connectors.Ali;

/// <summary>
/// Ali Translation Service.
/// </summary>
public sealed class AliTranslationService : ITranslateService
{
    private AliTranslationServiceConfig? _config;

    /// <inheritdoc/>
    public TranslateServiceConfig? Config => _config;

    /// <inheritdoc/>
    public ITranslateClient? Client { get; set; }

    /// <inheritdoc/>
    public void Initialize(TranslateServiceConfig? config)
    {
        if (config is not AliTranslationServiceConfig aliConfig)
        {
            throw new KernelException("Configuration is invalid");
        }

        if (_config != null && aliConfig.Equals(_config))
        {
            return;
        }

        _config = aliConfig;
        Client?.Dispose();
        Client = new AliTranslateClient(aliConfig);
    }

    /// <inheritdoc/>
    public TranslationLanguages GetSupportedLanguages()
    {
        var languages = new Dictionary<string, CultureInfo?>
        {
            { "auto", null },
            { "ab", new("ab") },
            { "sq", new("sq") },
            { "am", new("am") },
            { "ar", new("ar") },
            { "ak", new("ak") },
            { "an", new("an") },
            { "as", new("as") },
            { "az", new("az") },
            { "ast", new("ast") },
            { "ee", new("ee") },
            { "ay", new("ay") },
            { "ga", new("ga") },
            { "et", new("et") },
            { "oj", new("oj") },
            { "oc", new("oc") },
            { "or", new("or") },
            { "om", new("om") },
            { "os", new("os") },
            { "tpi", new("tpi") },
            { "ba", new("ba") },
            { "eu", new("eu") },
            { "be", new("be") },
            { "bm", new("bm") },
            { "pag", new("pag") },
            { "bg", new("bg") },
            { "se", new("se") },
            { "bem", new("bem") },
            { "byn", new("byn") },
            { "bi", new("bi") },
            { "bal", new("bal") },
            { "is", new("is") },
            { "pl", new("pl") },
            { "bs", new("bs") },
            { "fa", new("fa") },
            { "bho", new("bho") },
            { "br", new("br") },
            { "ch", new("ch") },
            { "cv", new("cv") },
            { "ts", new("ts") },
            { "tt", new("tt") },
            { "da", new("da") },
            { "shn", new("shn") },
            { "tet", new("tet") },
            { "de", new("de") },
            { "nds", new("nds") },
            { "sco", new("sco") },
            { "dv", new("dv") },
            { "dtp", new("dtp") },
            { "ru", new("ru") },
            { "fo", new("fo") },
            { "fr", new("fr") },
            { "sa", new("sa") },
            { "fil", new("fil") },
            { "fi", new("fi") },
            { "fj", new("fj") },
            { "fur", new("fur") },
            { "kg", new("kg") },
            { "km", new("km") },
            { "kl", new("kl") },
            { "ka", new("ka") },
            { "gu", new("gu") },
            { "gn", new("gn") },
            { "kk", new("kk") },
            { "ht", new("ht") },
            { "ko", new("ko") },
            { "ha", new("ha") },
            { "nl", new("nl") },
            { "hup", new("hup") },
            { "gil", new("gil") },
            { "rn", new("rn") },
            { "quc", new("quc") },
            { "ky", new("ky") },
            { "gl", new("gl") },
            { "ca", new("ca") },
            { "cs", new("cs") },
            { "kab", new("kab") },
            { "kn", new("kn") },
            { "kr", new("kr") },
            { "csb", new("csb") },
            { "kha", new("kha") },
            { "kw", new("kw") },
            { "xh", new("xh") },
            { "co", new("co") },
            { "mus", new("mus") },
            { "crh", new("crh") },
            { "tlh", new("tlh") },
            { "qu", new("qu") },
            { "ks", new("ks") },
            { "ku", new("ku") },
            { "la", new("la") },
            { "ltg", new("ltg") },
            { "lv", new("lv") },
            { "lo", new("lo") },
            { "lt", new("lt") },
            { "li", new("li") },
            { "ln", new("ln") },
            { "lg", new("lg") },
            { "lb", new("lb") },
            { "rue", new("rue") },
            { "rw", new("rw") },
            { "ro", new("ro") },
            { "rm", new("rm") },
            { "rom", new("rom") },
            { "jbo", new("jbo") },
            { "mg", new("mg") },
            { "gv", new("gv") },
            { "mt", new("mt") },
            { "mr", new("mr") },
            { "ml", new("ml") },
            { "ms", new("ms") },
            { "chm", new("chm") },
            { "mk", new("mk") },
            { "mh", new("mh") },
            { "mai", new("mai") },
            { "mfe", new("mfe") },
            { "mi", new("mi") },
            { "mn", new("mn") },
            { "bn", new("bn") },
            { "my", new("my") },
            { "hmn", new("hmn") },
            { "umb", new("umb") },
            { "nv", new("nv") },
            { "af", new("af") },
            { "ne", new("ne") },
            { "niu", new("niu") },
            { "no", new("no") },
            { "Pam", new("Pam") },
            { "pap", new("pap") },
            { "pa", new("pa") },
            { "pt", new("pt") },
            { "ps", new("ps") },
            { "ny", new("ny") },
            { "tw", new("tw") },
            { "chr", new("chr") },
            { "ja", new("ja") },
            { "sv", new("sv") },
            { "sm", new("sm") },
            { "sg", new("sg") },
            { "si", new("si") },
            { "hsb", new("hsb") },
            { "eo", new("eo") },
            { "sl", new("sl") },
            { "sw", new("sw") },
            { "so", new("so") },
            { "sk", new("sk") },
            { "tl", new("tl") },
            { "tg", new("tg") },
            { "ty", new("ty") },
            { "te", new("te") },
            { "ta", new("ta") },
            { "th", new("th") },
            { "to", new("to") },
            { "ti", new("ti") },
            { "tvl", new("tvl") },
            { "tyv", new("tyv") },
            { "tr", new("tr") },
            { "tk", new("tk") },
            { "wa", new("wa") },
            { "war", new("war") },
            { "cy", new("cy") },
            { "ve", new("ve") },
            { "vo", new("vo") },
            { "wo", new("wo") },
            { "udm", new("udm") },
            { "ur", new("ur") },
            { "uz", new("uz") },
            { "es", new("es") },
            { "ie", new("ie") },
            { "fy", new("fy") },
            { "szl", new("szl") },
            { "he", new("he") },
            { "hil", new("hil") },
            { "haw", new("haw") },
            { "el", new("el") },
            { "lfn", new("lfn") },
            { "sd", new("sd") },
            { "hu", new("hu") },
            { "sn", new("sn") },
            { "ceb", new("ceb") },
            { "syr", new("syr") },
            { "su", new("su") },
            { "hy", new("hy") },
            { "ace", new("ace") },
            { "iba", new("iba") },
            { "ig", new("ig") },
            { "io", new("io") },
            { "ilo", new("ilo") },
            { "iu", new("iu") },
            { "it", new("it") },
            { "yi", new("yi") },
            { "ia", new("ia") },
            { "hi", new("hi") },
            { "id", new("id") },
            { "inh", new("inh") },
            { "en", new("en") },
            { "yo", new("yo") },
            { "vi", new("vi") },
            { "zza", new("zza") },
            { "jv", new("jv") },
            { "zh", new("zh-hans") },
            { "zh-tw", new("zh-hant") },
            { "yue", new("yue") },
            { "zu", new("zu") },
        };

        return new(languages, languages.Skip(1).ToDictionary());
    }
}
