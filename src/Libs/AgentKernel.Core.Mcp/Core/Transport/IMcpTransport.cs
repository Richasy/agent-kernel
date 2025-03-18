// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Core.Mcp.Internal;
using System.Text.Json.Serialization.Metadata;

namespace Richasy.AgentKernel.Core.Mcp.Core;

internal interface IMcpTransport : IAsyncDisposable
{
    bool Running { get; }

    Task ConnectAsync(CancellationToken token = default);

    Task<T?> SendRequestAsync<T>(RpcRequest request, JsonTypeInfo<T> typeInfo, CancellationToken token = default);
}
