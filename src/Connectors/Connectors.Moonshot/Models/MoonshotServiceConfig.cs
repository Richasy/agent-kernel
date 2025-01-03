// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Moonshot.Models;

/// <summary>
/// Moonshot Service Configuration.
/// </summary>
public sealed class MoonshotServiceConfig(string key, string model) : AIServiceConfig(key, model)
{
}
