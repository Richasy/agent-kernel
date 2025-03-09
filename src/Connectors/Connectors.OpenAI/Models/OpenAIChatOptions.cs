// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using Richasy.AgentKernel.Core.OpenAI.Chat;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.OpenAI.Models;

/// <summary>
/// OpenAI Chat Options.
/// </summary>
public sealed class OpenAIChatOptions : ChatOptions
{
    /// <summary>
    /// The model to use for the chat.
    /// </summary>
    [JsonPropertyName("reasoning_effort")]
    public ChatReasoningEffortLevel? ReasoningEffort
    {
        get => field;
        set
        {
            if (field != value)
            {
                field = value;
                AdditionalProperties ??= [];
                AdditionalProperties.Add(nameof(ChatCompletionOptions.ReasoningEffortLevel), value);
            }
        }
    }

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
                AdditionalProperties.Add(nameof(ChatCompletionOptions.AllowParallelToolCalls), value);
            }
        }
    }
}
