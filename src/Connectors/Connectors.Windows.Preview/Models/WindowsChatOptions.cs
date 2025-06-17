// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using Microsoft.Windows.AI.ContentSafety;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Windows.Preview.Models;

/// <summary>
/// Chat options for Windows Chat Connector.
/// </summary>
public sealed class WindowsChatOptions : ChatOptions
{
    /// <summary>
    /// 输入审核方式.
    /// </summary>
    [JsonPropertyName("input_moderation")]
    public SeverityLevel? InputModeration
    {
        get => field;
        set
        {
            if (field != value)
            {
                field = value;
                AdditionalProperties ??= [];
                if (value == null)
                {
                    AdditionalProperties.Remove("input_moderation");
                }
                else
                {
                    AdditionalProperties["input_moderation"] = (int)value;
                }
            }
        }
    }

    /// <summary>
    /// 输出审核方式.
    /// </summary>
    public SeverityLevel? OutputModeration
    {
        get => field;
        set
        {
            if (field != value)
            {
                field = value;
                AdditionalProperties ??= [];
                if (value == null)
                {
                    AdditionalProperties.Remove("output_moderation");
                }
                else
                {
                    AdditionalProperties["output_moderation"] = (int)value;
                }
            }
        }
    }
}
