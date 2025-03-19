using System.Reflection;
using Richasy.AgentKernel.Core.Mcp.Protocol.Types;
using Richasy.AgentKernel.Core.Mcp.Server;
using Microsoft.Extensions.Options;

namespace Richasy.AgentKernel.Core.Mcp.Configuration;

internal sealed class McpServerOptionsSetup(IOptions<McpServerHandlers> serverHandlers) : IConfigureOptions<McpServerOptions>
{
    public void Configure(McpServerOptions options)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        var assemblyName = Assembly.GetEntryAssembly()?.GetName();
        options.ServerInfo = new Implementation
        {
            Name = assemblyName?.Name ?? "McpServer",
            Version = assemblyName?.Version?.ToString() ?? "1.0.0",
        };

        serverHandlers.Value.OverwriteWithSetHandlers(options);
    }
}
