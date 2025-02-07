// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Baidu.Core;
using Richasy.AgentKernel.Connectors.Baidu.Models;
using Richasy.AgentKernel.Models;
using Richasy.AgentKernel.Translation;
using RichasyKernel;
using System.Globalization;

namespace Richasy.AgentKernel.Connectors.Baidu;

/// <summary>
/// Baidu translation service.
/// </summary>
public sealed class BaiduTranslationService : ITranslateService
{
    private BaiduTranslationServiceConfig? _config;

    /// <inheritdoc/>
    public TranslateServiceConfig? Config => _config;

    /// <inheritdoc/>
    public ITranslateClient? Client { get; set; }

    /// <inheritdoc/>
    public void Initialize(TranslateServiceConfig? config)
    {
        if (config is not BaiduTranslationServiceConfig baiduConfig)
        {
            throw new KernelException("Configuration is invalid");
        }

        if (_config != null && baiduConfig.Equals(_config))
        {
            return;
        }

        _config = baiduConfig;
        Client?.Dispose();
        Client = new BaiduTranslateClient(baiduConfig);
    }

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
