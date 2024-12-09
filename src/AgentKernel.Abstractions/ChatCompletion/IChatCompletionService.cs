// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;

namespace Richasy.AgentKernel.ChatCompletion;

/// <summary>
/// Provides methods to complete chat.
/// </summary>
public interface IChatCompletionService
{
    /// <summary>
    /// Gets the client that is used to complete chat.
    /// </summary>
    IChatClient? Client { get; }

    /// <summary>
    /// Initialize the service.
    /// </summary>
    /// <param name="config">Service configuration.</param>
    void Initialize(AIServiceConfig config);
}
