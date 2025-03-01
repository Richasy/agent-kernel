// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Chat;

/// <summary>
/// Provides methods to complete chat.
/// </summary>
public interface IChatService
{
    /// <summary>
    /// Gets the client that is used to complete chat.
    /// </summary>
    IChatClient? Client { get; }

    /// <summary>
    /// Gets the configuration of the service.
    /// </summary>
    AIServiceConfig? Config { get; }

    /// <summary>
    /// Initialize the service.
    /// </summary>
    /// <param name="config">Service configuration.</param>
    void Initialize(AIServiceConfig? config);

    /// <summary>
    /// Get the predefined models.
    /// </summary>
    /// <returns>Model list.</returns>
    public IReadOnlyList<ChatModel> GetPredefinedModels();
}
