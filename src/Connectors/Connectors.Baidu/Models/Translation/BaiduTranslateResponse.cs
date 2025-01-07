// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Baidu.Models.Translation;

internal sealed class BaiduTranslateResponse
{
    [JsonPropertyName("from")]
    public string? From { get; set; }

    [JsonPropertyName("to")]
    public string? To { get; set; }

    [JsonPropertyName("trans_result")]
    public IList<TranslationResult>? Result { get; set; }

    internal sealed class TranslationResult
    {
        [JsonPropertyName("src")]
        public string? Source { get; set; }

        [JsonPropertyName("dst")]
        public string? Result { get; set; }
    }
}
