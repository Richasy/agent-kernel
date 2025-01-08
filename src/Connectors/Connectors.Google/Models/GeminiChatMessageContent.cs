// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;

namespace Richasy.AgentKernel.Connectors.Google.Models;

internal sealed class GeminiChatMessageContent : ChatMessage
{
    /// <summary>
    /// The metadata associated with the content.
    /// </summary>
    public GeminiMetadata? Metadata { get; set; }
}
