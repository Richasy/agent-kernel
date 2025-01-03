// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json;

namespace Richasy.AgentKernel;

/// <summary>
/// Json工具.
/// </summary>
public static class JsonToolkit
{
    /// <summary>
    /// 将Json字符串转换为指定类型的对象.
    /// </summary>
    /// <returns>对象字典.</returns>
    /// <exception cref="ArgumentException"></exception>
    public static Dictionary<string, object?> JsonElementToDictionary(JsonElement element)
    {
        if (element.ValueKind != JsonValueKind.Object)
        {
            throw new ArgumentException("The JsonElement must be of Object type", nameof(element));
        }

        var dictionary = new Dictionary<string, object?>();
        foreach (var property in element.EnumerateObject())
        {
            dictionary[property.Name] = ConvertJsonElement(property.Value);
        }

        return dictionary;
    }

    /// <summary>
    /// 将Json字符串转换为指定类型的对象.
    /// </summary>
    /// <returns>对象</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static object? ConvertJsonElement(JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                return JsonElementToDictionary(element);
            case JsonValueKind.Array:
                var list = new List<object?>();
                foreach (var item in element.EnumerateArray())
                {
                    list.Add(ConvertJsonElement(item));
                }

                return list;
            case JsonValueKind.String:
                return element.GetString();
            case JsonValueKind.Number:
                if (element.TryGetInt32(out var intValue))
                {
                    return intValue;
                }

                if (element.TryGetInt64(out var longValue))
                {
                    return longValue;
                }

                if (element.TryGetDouble(out var doubleValue))
                {
                    return doubleValue;
                }

                throw new InvalidOperationException("Unsupported number type");
            case JsonValueKind.True:
                return true;
            case JsonValueKind.False:
                return false;
            case JsonValueKind.Null:
            case JsonValueKind.Undefined:
                return null;
            default:
                throw new InvalidOperationException("Unsupported JsonValueKind");
        }
    }
}
