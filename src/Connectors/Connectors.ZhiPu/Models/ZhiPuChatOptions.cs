// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.ZhiPu.Models;

/// <summary>
/// 智谱对话选项.
/// </summary>
public sealed class ZhiPuChatOptions : ChatOptions
{
    /// <summary>
    /// 搜索参数。
    /// </summary>
    [JsonPropertyName("search")]
    public ZhiPuWebSearchParameters? Search { get; set; }

    /// <summary>
    /// 检索参数。
    /// </summary>
    [JsonPropertyName("retrieval")]
    public ZhiPuRetrievalParameters? Retrieval { get; set; }
}
