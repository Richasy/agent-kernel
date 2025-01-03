// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Tencent.Models;

/// <summary>
/// Hunyuan service configuration.
/// </summary>
public sealed class HunyuanServiceConfig(string key, string model) : AIServiceConfig(key, model)
{
}
