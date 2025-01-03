// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.OpenRouter.Models;

/// <summary>
/// OpenRouter configuration.
/// </summary>
public sealed class OpenRouterServiceConfig(string key, string model) : AIServiceConfig(key, model)
{
}
