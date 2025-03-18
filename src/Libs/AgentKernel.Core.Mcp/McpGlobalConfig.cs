using Richasy.AgentKernel.Core.Mcp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Richasy.AgentKernel.Core.Mcp;

/// <summary>
/// 全局配置.
/// </summary>
public static class McpGlobalConfig
{
    /// <summary>
    /// 在 Windows 上使用 cmd 作为默认命令.
    /// </summary>
    /// <remarks>
    /// 开启后，会在运行时将 <see cref="McpServerConfig"/> 里的 <see cref="McpServerConfig.Command"/>
    /// 属性替换为 <c>cmd.exe</c>，并在 <see cref="McpServerConfig.Arguments"/> 属性的首位插入 <c>/c</c> 参数和原命令.
    /// </remarks>
    public static bool UseCmdAsDefaultCommand { get; set; }
}
