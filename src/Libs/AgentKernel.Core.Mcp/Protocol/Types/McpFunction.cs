using Microsoft.Extensions.AI;
using Richasy.AgentKernel.Core.Mcp.Client;
using Richasy.AgentKernel.Core.Mcp.Utils;
using Richasy.AgentKernel.Core.Mcp.Utils.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Core.Mcp.Protocol.Types;

/// <summary>
/// Represents a tool that the server is capable of calling. Part of the ListToolsResponse.
/// <see href="https://github.com/modelcontextprotocol/specification/blob/main/schema/2024-11-05/schema.json">See the schema for details</see>
/// </summary>
[JsonConverter(typeof(McpToolJsonConverter))]
public class McpTool(string name, string description, JsonElement? inputSchema) : AIFunction
{
    /// <inheritdoc/>
    public override string Name => name;

    /// <inheritdoc/>
    public override string Description => description;

    /// <inheritdoc/>
    public override JsonElement JsonSchema => inputSchema ?? JsonDocument.Parse("{}").RootElement;

    /// <summary>
    /// The client that this tool is associated with.
    /// </summary>
    [JsonIgnore]
    public IMcpClient? Client { get; set; }

    /// <inheritdoc/>
    protected override async ValueTask<object?> InvokeCoreAsync(AIFunctionArguments arguments, CancellationToken cancellationToken)
    {
        Throw.IfNull(arguments);
        Throw.IfNull(Client);

        Dictionary<string, object> argDict = [];
        foreach (var arg in arguments)
        {
            if (arg.Value is not null)
            {
                argDict[arg.Key] = arg.Value;
            }
        }

        CallToolResponse result = await Client.CallToolAsync(Name, argDict, cancellationToken).ConfigureAwait(false);
        return JsonSerializer.SerializeToElement(result, JsonSerializerOptionsExtensions.JsonContext.Default.CallToolResponse);
    }
}
