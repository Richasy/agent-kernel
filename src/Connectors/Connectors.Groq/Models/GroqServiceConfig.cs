// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Groq.Models;

/// <summary>
/// Groq Service Configuration.
/// </summary>
public sealed class GroqServiceConfig(string key, string model) : AIServiceConfig(key, model)
{
}
