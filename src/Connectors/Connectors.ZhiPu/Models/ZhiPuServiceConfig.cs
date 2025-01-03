// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.ZhiPu.Models;

/// <summary>
/// Configuration of the ZhiPu service.
/// </summary>
public sealed class ZhiPuServiceConfig(string key, string model) : AIServiceConfig(key, model)
{
}
