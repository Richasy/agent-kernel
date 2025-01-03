// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Perplexity.Models;

/// <summary>
/// Perplexity configuration.
/// </summary>
public sealed class PerplexityServiceConfig(string key, string model) : AIServiceConfig(key, model)
{
}
