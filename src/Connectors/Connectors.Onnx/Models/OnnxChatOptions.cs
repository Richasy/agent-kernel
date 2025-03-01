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
}
