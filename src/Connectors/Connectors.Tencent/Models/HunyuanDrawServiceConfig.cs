// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Tencent.Models;

/// <summary>
/// Hunyuan draw service configuration.
/// </summary>
public sealed class HunyuanDrawServiceConfig(string secretKey, string secretId, string? model) : AIServiceConfig(secretKey, model)
{
    /// <summary>
    /// Tencent cloud secret id.
    /// </summary>
    public string SecretId { get; set; } = secretId;
}
