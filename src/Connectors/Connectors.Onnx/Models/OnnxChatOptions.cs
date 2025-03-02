// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Onnx.Models;

/// <summary>
/// ONNX 服务配置.
/// </summary>
public sealed class OnnxChatOptions : ChatOptions
{
    /// <summary>
    /// 最小长度.
    /// </summary>
    [JsonPropertyName("min_length")]
    public int? MinLength
    {
        get => field;
        set
        {
            if (field != value)
            {
                field = value;
                AdditionalProperties ??= [];
                AdditionalProperties["min_length"] = BinaryData.FromString(JsonSerializer.Serialize(value, JsonGenContext.Default.Int32));
            }
        }
    }

    /// <summary>
    /// 是否采取随机采样.
    /// </summary>
    [JsonPropertyName("do_sample")]
    public bool? DoSample
    {
        get => field;
        set
        {
            if (field != value)
            {
                field = value;
                AdditionalProperties ??= [];
                AdditionalProperties["do_sample"] = BinaryData.FromString(JsonSerializer.Serialize(value, JsonGenContext.Default.Boolean));
            }
        }
    }

    /// <summary>
    /// 系统提示词模板.
    /// </summary>
    [JsonPropertyName("system_template")]
    public string? SystemTemplate
    {
        get => field;
        set
        {
            if (field != value)
            {
                field = value;
                AdditionalProperties ??= [];
                AdditionalProperties["system_template"] = BinaryData.FromString(JsonSerializer.Serialize(value, JsonGenContext.Default.String));
            }
        }
    }

    /// <summary>
    /// 用户消息模板.
    /// </summary>
    [JsonPropertyName("user_template")]
    public string? UserTemplate
    {
        get => field;
        set
        {
            if (field != value)
            {
                field = value;
                AdditionalProperties ??= [];
                AdditionalProperties["user_template"] = BinaryData.FromString(JsonSerializer.Serialize(value, JsonGenContext.Default.String));
            }
        }
    }

    /// <summary>
    /// 助手消息模板.
    /// </summary>
    [JsonPropertyName("assistant_template")]
    public string? AssistantTemplate
    {
        get => field;
        set
        {
            if (field != value)
            {
                field = value;
                AdditionalProperties ??= [];
                AdditionalProperties["assistant_template"] = BinaryData.FromString(JsonSerializer.Serialize(value, JsonGenContext.Default.String));
            }
        }
    }

    /// <summary>
    /// 提示词模板.
    /// </summary>
    [JsonPropertyName("prompt_template")]
    public string? PromptTemplate
    {
        get => field;
        set
        {
            if (field != value)
            {
                field = value;
                AdditionalProperties ??= [];
                AdditionalProperties["prompt_template"] = BinaryData.FromString(JsonSerializer.Serialize(value, JsonGenContext.Default.String));
            }
        }
    }
}
