// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Tencent.Models;

/// <summary>
/// 混元对话选项.
/// </summary>
public sealed class HunyuanChatOptions : ChatOptions
{
    /// <summary>
    /// 搜索引文角标开关。
    /// 说明：
    /// 配合 enable_enhancement 和 search_info 参数使用。打开后，回答中命中搜索的结果会在片段后增加角标标志，对应 search_info 列表中的链接。
    /// false：开关关闭，true：开关打开。
    /// 未传值时默认开关关闭（false）。
    /// </summary>
    [JsonPropertyName("citation")]
    public bool? Citation
    {
        get => field;
        set
        {
            if (field != value)
            {
                field = value;
                AdditionalProperties ??= [];
                AdditionalProperties.Add("citation", value);
            }
        }
    }

    /// <summary>
    /// 是否开启深度研究该问题，默认是false，在值为true且命中深度研究该问题时，会返回深度研究该问题信息。
    /// </summary>
    [JsonPropertyName("enable_deep_search")]
    public bool? EnableDeepSearch
    {
        get => field;
        set
        {
            if (field != value)
            {
                field = value;
                AdditionalProperties ??= [];
                AdditionalProperties.Add("enable_deep_search", value);
            }
        }
    }

    /// <summary>
    /// 功能增强（如搜索）开关。
    /// </summary>
    [JsonPropertyName("enable_enhancement")]
    public bool? EnableEnhancement
    {
        get => field;
        set
        {
            if (field != value)
            {
                field = value;
                AdditionalProperties ??= [];
                AdditionalProperties.Add("enable_enhancement", value);
            }
        }
    }

    /// <summary>
    /// 多媒体开关。该参数目前仅对白名单内用户生效
    /// </summary>
    [JsonPropertyName("enable_multimedia")]
    public bool? EnableMultimedia
    {
        get => field;
        set
        {
            if (field != value)
            {
                field = value;
                AdditionalProperties ??= [];
                AdditionalProperties.Add("enable_multimedia", value);
            }
        }
    }

    /// <summary>
    /// 是否开启极速版搜索，默认 false，不开启。
    /// </summary>
    [JsonPropertyName("enable_speed_search")]
    public bool? EnableSpeedSearch
    {
        get => field;
        set
        {
            if (field != value)
            {
                field = value;
                AdditionalProperties ??= [];
                AdditionalProperties.Add("enable_speed_search", value);
            }
        }
    }

    /// <summary>
    /// 是否强制开启功能增强（如搜索）开关。
    /// </summary>
    [JsonPropertyName("force_search_enhancement")]
    public bool? ForceSearchEnhancement
    {
        get => field;
        set
        {
            if (field != value)
            {
                field = value;
                AdditionalProperties ??= [];
                AdditionalProperties.Add("force_search_enhancement", value);
            }
        }
    }

    /// <summary>
    /// 在值为true且命中搜索时，接口会返回 search_info.
    /// </summary>
    [JsonPropertyName("search_info")]
    public bool? SearchInfo
    {
        get => field;
        set
        {
            if (field != value)
            {
                field = value;
                AdditionalProperties ??= [];
                AdditionalProperties.Add("search_info", value);
            }
        }
    }
}
