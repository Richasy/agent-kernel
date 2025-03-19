namespace Richasy.AgentKernel.Core.Mcp.Server;

/// <summary>
/// Container for the request context.
/// </summary>
/// <typeparam name="TParams">Type of the request parameters</typeparam>
public record RequestContext<TParams>(IMcpServer Server, TParams? Params)
{

}
