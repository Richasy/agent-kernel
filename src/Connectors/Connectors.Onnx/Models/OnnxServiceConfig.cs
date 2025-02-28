// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Onnx.Models;

/// <summary>
/// ONNX 服务配置.
/// </summary>
/// <param name="modelFolder"></param>
public sealed class OnnxServiceConfig(string modelFolder) : AIServiceConfig(string.Empty, modelFolder)
{
    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is OnnxServiceConfig config && base.Equals(obj) && Model == config.Model;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Model);
}
