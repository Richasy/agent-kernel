// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using Richasy.AgentKernel.Chat;
using Richasy.AgentKernel.Connectors.Onnx.Core;
using Richasy.AgentKernel.Connectors.Onnx.Models;
using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Onnx;

/// <summary>
/// ONNX Chat Completion Service.
/// </summary>
public sealed class OnnxChatService : IChatService
{
    private OnnxServiceConfig? _config;

    /// <inheritdoc/>
    public IChatClient? Client { get; set; }

    /// <inheritdoc/>
    public AIServiceConfig? Config => _config;

    /// <inheritdoc/>
    public IReadOnlyList<ChatModel> GetPredefinedModels() => [];

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig? config)
    {
        if (config is not OnnxServiceConfig onnxConfig)
        {
            throw new ArgumentException("The configuration is not valid.", nameof(config));
        }

        if (_config != null && onnxConfig.Equals(_config) && onnxConfig.UseCuda == _config.UseCuda)
        {
            return;
        }

        _config = onnxConfig;
        Client?.Dispose();
        Client = new OnnxChatClient(_config.Model, _config.UseCuda);
    }
}
