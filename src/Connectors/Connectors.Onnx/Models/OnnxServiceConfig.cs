// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Onnx.Models;

/// <summary>
/// ONNX 服务配置.
/// </summary>
public class OnnxServiceConfig(string modelFolder, bool useCuda) : AIServiceConfig(string.Empty, modelFolder)
{
    /// <summary>
    /// 是否使用CUDA进行推理加速.
    /// </summary>
    public bool UseCuda { get; set; } = useCuda;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is OnnxServiceConfig config && base.Equals(obj) && Model == config.Model;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Model);
}
