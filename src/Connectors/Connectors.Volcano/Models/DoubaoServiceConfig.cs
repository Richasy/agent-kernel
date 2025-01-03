// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Volcano.Models;

/// <summary>
/// 豆包服务配置.
/// </summary>
public sealed class DoubaoServiceConfig(string key, string model) : AIServiceConfig(key, model)
{
}
