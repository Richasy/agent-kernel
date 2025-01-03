// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.XAI.Models;

/// <summary>
/// XAI Service Configuration.
/// </summary>
public sealed class XAIServiceConfig(string key, string model) : AIServiceConfig(key, model)
{
}
