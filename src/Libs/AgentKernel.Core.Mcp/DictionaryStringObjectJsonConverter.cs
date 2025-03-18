// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Core.Mcp;

internal sealed class DictionaryStringObjectJsonConverter : JsonConverter<Dictionary<string, object?>?>
{
    public override Dictionary<string, object?>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        var dictionary = new Dictionary<string, object?>();

        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return dictionary;
            }

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }

            var propertyName = reader.GetString()!;

            reader.Read();

            switch (reader.TokenType)
            {
                case JsonTokenType.String:
                    dictionary[propertyName] = reader.GetString()!;
                    break;
                case JsonTokenType.Number:
                    if (reader.TryGetInt64(out var l))
                    {
                        dictionary[propertyName] = l;
                    }
                    else if (reader.TryGetDouble(out var d))
                    {
                        dictionary[propertyName] = d;
                    }

                    break;
                case JsonTokenType.True:
                case JsonTokenType.False:
                    dictionary[propertyName] = reader.GetBoolean();
                    break;
                case JsonTokenType.Null:
                    dictionary[propertyName] = null;
                    break;
                default:
                    throw new JsonException();
            }
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<string, object?>? value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();

        foreach (var kvp in value)
        {
            writer.WritePropertyName(kvp.Key);
            if (kvp.Value == null)
            {
                writer.WriteNullValue();
            }
            else if (kvp.Value is string s)
            {
                writer.WriteStringValue(s);
            }
            else if (kvp.Value is long l)
            {
                writer.WriteNumberValue(l);
            }
            else if (kvp.Value is double d)
            {
                writer.WriteNumberValue(d);
            }
            else if (kvp.Value is bool b)
            {
                writer.WriteBooleanValue(b);
            }
            else
            {
                throw new NotSupportedException($"Type {kvp.Value.GetType()} is not supported");
            }
        }

        writer.WriteEndObject();
    }
}
