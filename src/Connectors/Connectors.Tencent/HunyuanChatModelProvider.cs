// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.ChatCompletion;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Tencent;

/// <summary>
/// Provides chat models for the Hunyuan connector.
/// </summary>
public sealed class HunyuanChatModelProvider : IChatModelProvider
{
    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetModels() =>
    [
        new("hunyuan-turbo-latest", "Hunyuan Turbo Latest", toolSupport: true),
        new("hunyuan-large", "Hunyuan Large"),
        new("hunyuan-large-longcontext", "Hunyuan Large Long Context"),
        new("hunyuan-standard-256k", "Hunyuan Standard 256K"),
        new("hunyuan-standard", "Hunyuan Standard"),
        new("hunyuan-lite", "Hunyuan Lite"),
        new("hunyuan-functioncall", "Hunyuan Function Call"),
    ];
}
