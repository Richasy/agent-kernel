// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Onnx.Models;

/// <summary>
/// ONNX 服务配置.
/// </summary>
public class OnnxServiceConfig : AIServiceConfig
{
#if USE_CUDA
    /// <summary>
    /// Initializes a new instance of the <see cref="OnnxServiceConfig"/> class.
    /// </summary>
    public OnnxServiceConfig(string modelFolder, bool useCuda)
        : base(string.Empty, modelFolder)
    {
        UseCuda = useCuda;
    }

    /// <summary>
    /// 是否使用CUDA进行推理加速.
    /// </summary>
    public bool UseCuda { get; set; }
#else
    /// <summary>
    /// Initializes a new instance of the <see cref="OnnxServiceConfig"/> class.
    /// </summary>
    public OnnxServiceConfig(string modelFolder)
        : base(string.Empty, modelFolder)
    {
    }
#endif

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is OnnxServiceConfig config && base.Equals(obj) && Model == config.Model;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Model);
}
