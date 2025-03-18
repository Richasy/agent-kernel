// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Logging;
using Richasy.AgentKernel.Core.Mcp.Models;
using System.Diagnostics;

namespace Richasy.AgentKernel.Core.Mcp.Core;

/// <summary>
/// MCP 客户端.
/// </summary>
public sealed partial class McpClient : IAsyncDisposable
{
    /// <summary>
    /// 初始化 <see cref="McpClient"/> 类的新实例.
    /// </summary>
    /// <param name="id">Client id.</param>
    /// <param name="config">MCP 服务配置.</param>
    /// <param name="transport">传输类型.</param>
    /// <param name="logger">日志记录.</param>
    public McpClient(string id, McpServerConfig config, McpTransportType transport, ILogger logger = null)
    {
        _id = id;
        _logger = logger;
        _config = config;
        _transport = transport;
    }

    /// <summary>
    /// 运行 MCP 客户端.
    /// </summary>
    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        if (_transportInstance != null)
        {
            throw new InvalidOperationException("Client is already running.");
        }

        if (_transport == McpTransportType.StandardInputOutput)
        {
            _transportInstance = new StdioTransport(_id, _config, _logger);
        }

        if (_transportInstance != null)
        {
            try
            {
                _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                await _transportInstance.ConnectAsync(_cancellationTokenSource.Token).ConfigureAwait(false);
            }
            catch (Exception)
            {
                await DisposeAsync().ConfigureAwait(false);
                throw;
            }
        }
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        if (_transportInstance != null)
        {
            await _transportInstance.DisposeAsync();
            _transportInstance = null;
        }
    }
}
