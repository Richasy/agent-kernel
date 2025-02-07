// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Tencent.Core;
using Richasy.AgentKernel.Connectors.Tencent.Models;
using Richasy.AgentKernel.Models;
using Richasy.AgentKernel.Translation;
using RichasyKernel;
using System.Globalization;

namespace Richasy.AgentKernel.Connectors.Tencent;

/// <summary>
/// Tencent Translation Service.
/// </summary>
public sealed class TencentTranslationService : ITranslateService
{
    private TencentTranslationServiceConfig? _config;

    /// <inheritdoc/>
    public TranslateServiceConfig? Config => _config;

    /// <inheritdoc/>
    public ITranslateClient? Client { get; set; }

    /// <inheritdoc/>
    public void Initialize(TranslateServiceConfig? config)
    {
        if (config is not TencentTranslationServiceConfig tencentConfig)
        {
            throw new KernelException("Configuration is invalid");
        }

        if (_config != null && tencentConfig.Equals(_config))
        {
            return;
        }

        _config = tencentConfig;
        Client?.Dispose();
        Client = new TencentTranslateClient(tencentConfig);
    }

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
