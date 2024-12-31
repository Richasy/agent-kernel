// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.IFlyTek.Models;

/// <summary>
/// Spark service configuration.
/// </summary>
public sealed class SparkServiceConfig(string key, string model) : AIServiceConfig(key, model)
{
}
