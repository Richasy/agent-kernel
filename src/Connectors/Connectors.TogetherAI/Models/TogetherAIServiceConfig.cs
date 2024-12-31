// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.TogetherAI.Models;

/// <summary>
/// Together.AI Service Configuration.
/// </summary>
public sealed class TogetherAIServiceConfig(string key, string model) : AIServiceConfig(key, model)
{
}
