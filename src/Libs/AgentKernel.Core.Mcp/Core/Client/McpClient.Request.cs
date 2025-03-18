// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Core.Mcp.Internal;

namespace Richasy.AgentKernel.Core.Mcp.Core;

public sealed partial class McpClient
{
    /// <summary>
    /// Ping 服务器.
    /// </summary>
    /// <returns>Ping 结果.</returns>
    public async Task<bool> PingAsync(CancellationToken cancellationToken = default)
    {
        Interlocked.Increment(ref _requestId);
        var request = new RpcRequest(_requestId, "ping");
        using var pingCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var pingResponse = await _transportInstance.SendRequestAsync(request, JsonGenContext.Default.RpcResponse, pingCts.Token);
        return pingResponse != null;
    }
}
