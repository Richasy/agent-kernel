// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Baidu;

/// <summary>
/// Provides chat models for the Ernie connector.
/// </summary>
public sealed class ErnieChatModelProvider : IChatModelProvider
{
    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetModels() =>
    [
        new("ernie-4.0-8k-latest", "ERNIE 4.0", toolSupport:true),
        new("ernie-4.0-turbo-8k-latest", "ERNIE 4.0 Turbo 8K", toolSupport:true),
        new("ernie-4.0-turbo-128k", "ERNIE 4.0 Turbo 128K", toolSupport:true),
        new("ernie-3.5-8k", "ERNIE 3.5 8K", toolSupport:true),
        new("ernie-3.5-128k", "ERNIE 3.5 128K", toolSupport:true),
        new("ernie-speed-8k", "ERNIE Speed 8K"),
        new("ernie-speed-128k", "ERNIE Speed 128K"),
        new("ernie-speed-pro-128k", "ERNIE Speed Pro 128K", toolSupport: true),
        new("ernie-lite-8k", "ERNIE Lite 8K"),
        new("ernie-lite-pro-128k", "ERNIE Lite Pro 128K", toolSupport: true),
        new("ernie-tiny-8k", "ERNIE Tiny"),
        new("ernie-character-8k", "ERNIE Character"),
        new("ernie-novel-8k", "ERNIE-Novel-8K"),
    ];
}
