// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Ali.Models;

/// <summary>
/// 通义千问对话选项.
/// </summary>
public sealed class QwenChatOptions : ChatOptions
{
    /// <summary>
    /// 是否开启并行工具调用，开启后，模型会根据问题返回多个工具调用指令. 默认情况下，该值为 false. 只会返回一个工具调用指令.
    /// </summary>
    [JsonPropertyName("parallel_tool_calls")]
    public bool ParallelToolCalls
    {
        get => field;
        set
        {
            if (field != value)
            {
                field = value;
                AdditionalProperties ??= [];
                AdditionalProperties.Add("parallel_tool_calls", BinaryData.FromString(JsonSerializer.Serialize(value, JsonGenContext.Default.Boolean)));
            }
        }
    }

    /// <summary>
    /// 模型在生成文本时是否使用互联网搜索结果进行参考.
    /// </summary>
    [JsonPropertyName("enable_search")]
    public bool EnableSearch
    {
        get => field;
        set
        {
            if (field != value)
            {
                field = value;
                AdditionalProperties ??= [];
                AdditionalProperties.Add("enable_search", BinaryData.FromString(JsonSerializer.Serialize(value, JsonGenContext.Default.Boolean)));
            }
        }
    }
}
