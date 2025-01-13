// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.IFlyTek.Models;

/// <summary>
/// Represents a function parameter that can be passed to an SparkDesk function tool call.
/// </summary>
internal sealed class SparkMessage
{
    /// <summary>
    /// Text messages.
    /// </summary>
    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonRequired]
    public IList<SparkTextMessage>? Text { get; set; }

    internal class SparkTextMessage
    {
        /// <summary>
        /// Message content.
        /// </summary>
        [JsonPropertyName("content")]
        public string? Content { get; set; }

        /// <summary>
        /// The producer of the content. Must be either 'user' or 'assistant' or 'system' pr 'tool'.
        /// </summary>
        /// <remarks>Useful to set for multi-turn conversations, otherwise can be left blank or unset.</remarks>
        [JsonPropertyName("role")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonRequired]
        public ChatRole? Role { get; set; }
    }
}
