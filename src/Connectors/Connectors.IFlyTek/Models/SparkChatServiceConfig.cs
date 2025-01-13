// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.IFlyTek.Models;

/// <summary>
/// Spark service configuration.
/// </summary>
public sealed class SparkChatServiceConfig(string key, string model) : AIServiceConfig(key, model)
{
}
