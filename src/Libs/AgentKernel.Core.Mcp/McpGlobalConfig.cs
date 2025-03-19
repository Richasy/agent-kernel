using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Richasy.AgentKernel.Core.Mcp;

/// <summary>
/// Global configuration for the MCP.
/// </summary>
public static class McpGlobalConfig
{
    /// <summary>
    /// Use CMD.exe as the default client command.
    /// Original command will move to the arguments.
    /// </summary>
    public static bool UseCmdAsDefaultClientCommand { get; set; }
}
