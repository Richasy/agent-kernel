// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Baidu.Models;

/// <summary>
/// 文心一言服务配置.
/// </summary>
public sealed class ErnieServiceConfig(string key, string model) : AIServiceConfig(key, model)
{
}
