// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Logging;
using Richasy.AgentKernel.Core.Mcp.Models;
using System.Diagnostics;

namespace Richasy.AgentKernel.Core.Mcp.Core;

public sealed partial class McpClient
{
    private readonly string _id;
    private readonly ILogger _logger;
    private readonly McpServerConfig _config;
    private readonly McpTransportType _transport;
    private IMcpTransport? _transportInstance;
    private CancellationTokenSource? _cancellationTokenSource;

    private int _requestId;
}
