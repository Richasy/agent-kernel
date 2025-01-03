// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.SiliconFlow.Models;

/// <summary>
/// 硅基流动配置.
/// </summary>
public sealed class SiliconFlowServiceConfig(string key, string model) : AIServiceConfig(key, model)
{
}
