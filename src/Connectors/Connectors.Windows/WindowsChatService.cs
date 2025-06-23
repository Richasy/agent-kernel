// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.Windows.Core;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Windows;

/// <summary>
/// Windows Chat Completion Service.
/// </summary>
public sealed class WindowsChatService : IChatService
{
    /// <inheritdoc/>
    public IChatClient? Client { get; set; }

    /// <inheritdoc/>
    public AIServiceConfig? Config => null;

    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetPredefinedModels() => [];

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig? config)
    {
        if (Client != null)
        {
            return;
        }

        Client = new WindowsChatClient();
    }
}
