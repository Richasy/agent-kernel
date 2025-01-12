// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.IFlyTek;

/// <summary>
/// Provides chat models for the Spark connector.
/// </summary>
public sealed class SparkChatModelProvider : IChatModelProvider
{
    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetModels() =>
    [
        new("lite", "Spark Lite"),
        new("generalv3", "Spark Pro"),
        new("pro-128k", "Spark Pro 128K"),
        new("generalv3.5", "Spark Max", toolSupport : true),
        new("max-32k", "Spark Max 32K", toolSupport: true),
        new("4.0Ultra", "Spark 4.0 Ultra", toolSupport: true),
    ];
}
