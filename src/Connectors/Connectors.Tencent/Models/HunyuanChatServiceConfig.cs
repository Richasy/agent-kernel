// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Tencent.Models;

/// <summary>
/// Hunyuan chat service configuration.
/// </summary>
public sealed class HunyuanChatServiceConfig(string key, string model) : AIServiceConfig(key, model)
{
}
