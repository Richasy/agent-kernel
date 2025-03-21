// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.XAI.Models;

/// <summary>
/// XAI Service Configuration.
/// </summary>
public sealed class XAIServiceConfig(string key, string model) : AIServiceConfig(key, model)
{
    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is XAIServiceConfig config && base.Equals(obj) && AccessKey == config.AccessKey && Model == config.Model;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), AccessKey, Model);
}
