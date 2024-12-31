// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Ali.Models;

/// <summary>
/// 千问服务配置.
/// </summary>
public sealed class QwenServiceConfig(string key, string model) : AIServiceConfig(key, model)
{
}
