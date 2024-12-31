// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel;

namespace Connectors.DeepSeek.Models;

/// <summary>
/// DeepSeek Service Configuration.
/// </summary>
public sealed class DeepSeekServiceConfig(string key, string model) : AIServiceConfig(key, model)
{
}
