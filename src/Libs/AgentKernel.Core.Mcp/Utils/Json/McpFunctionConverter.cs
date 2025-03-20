using Microsoft.Extensions.AI;
using Richasy.AgentKernel.Core.Mcp.Protocol.Types;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Core.Mcp.Utils.Json;

/// <summary>
/// Json converter for <see cref="McpTool"/>.
/// </summary>
public sealed class McpToolJsonConverter : JsonConverter<McpTool>
{
    /// <inheritdoc/>
    public override McpTool? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        string? name = null;
        string? description = null;
        JsonElement? inputSchema = null;
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new JsonException("Tool name is required");
                }

                if (inputSchema != null)
                {
                    var typeProp = inputSchema!.Value.GetProperty("type");
                    if (typeProp.GetString() != "object")
                    {
                        throw new JsonException("Input schema must be an object");
                    }
                }

                return new McpTool(name!, description, inputSchema);
            }

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }

            string propertyName = reader.GetString()!;
            reader.Read();

            switch (propertyName)
            {
                case "name":
                    name = reader.GetString();
                    break;
                case "description":
                    description = reader.GetString();
                    break;
                case "inputSchema":
                    inputSchema = JsonDocument.ParseValue(ref reader).RootElement;
                    break;
                default:
                    throw new JsonException("Unknown property");
            }
        }

        throw new JsonException();
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, McpTool value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("name", value.Name);
        if (value.Description != null)
        {
            writer.WriteString("description", value.Description);
        }

        if (value.JsonSchema.GetPropertyCount() > 0)
        {
            writer.WritePropertyName("inputSchema");
            value.JsonSchema.WriteTo(writer);
        }

        writer.WriteEndObject();
    }
}
