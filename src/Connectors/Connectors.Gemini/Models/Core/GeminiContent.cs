// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Gemini.Models.Core;

/// <summary>
/// The base structured datatype containing multi-part content of a message.
/// </summary>
internal sealed class GeminiContent
{
    /// <summary>
    /// Ordered Parts that constitute a single message. Parts may have different MIME types.
    /// </summary>
    [JsonPropertyName("parts")]
    public IList<GeminiPart>? Parts { get; set; }

    /// <summary>
    /// Optional. The producer of the content. Must be either 'user' or 'model' or 'function'.
    /// </summary>
    /// <remarks>Useful to set for multi-turn conversations, otherwise can be left blank or unset.</remarks>
    [JsonPropertyName("role")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ChatRole? Role { get; set; }
}
