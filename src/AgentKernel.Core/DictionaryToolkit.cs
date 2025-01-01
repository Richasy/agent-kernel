// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Globalization;
using System.Text;

namespace Richasy.AgentKernel;

/// <summary>
/// 字典工具.
/// </summary>
public static class DictionaryToolkit
{
    /// <summary>
    /// 写入字典.
    /// </summary>
    public static void WriteDictionary(StringBuilder stringBuilder, IDictionary<string, object?> dictionary)
    {
        stringBuilder.Append('{');
        var first = true;
        foreach (var kvp in dictionary)
        {
            if (!first)
            {
                stringBuilder.Append(',');
            }

            first = false;

            WriteString(stringBuilder, kvp.Key);
            stringBuilder.Append(':');
            WriteValue(stringBuilder, kvp.Value);
        }

        stringBuilder.Append('}');
    }

    private static void WriteValue(StringBuilder stringBuilder, object? value)
    {
        if (value == null)
        {
            stringBuilder.Append("null");
        }
        else if (value is string strValue)
        {
            WriteString(stringBuilder, strValue);
        }
        else if (value is int intValue)
        {
            stringBuilder.Append(intValue);
        }
        else if (value is long longValue)
        {
            stringBuilder.Append(longValue);
        }
        else if (value is double doubleValue)
        {
            stringBuilder.Append(doubleValue);
        }
        else if (value is bool boolValue)
        {
            stringBuilder.Append(boolValue.ToString().ToLower(CultureInfo.CurrentCulture));
        }
        else if (value is IDictionary<string, object?> nestedDictionary)
        {
            WriteDictionary(stringBuilder, nestedDictionary);
        }
        else if (value is List<object> list)
        {
            WriteList(stringBuilder, list);
        }
        else
        {
            throw new InvalidOperationException($"Unsupported type: {value.GetType()}");
        }
    }

    private static void WriteList(StringBuilder stringBuilder, List<object> list)
    {
        stringBuilder.Append('[');
        var first = true;
        foreach (var item in list)
        {
            if (!first)
            {
                stringBuilder.Append(',');
            }

            first = false;

            WriteValue(stringBuilder, item);
        }

        stringBuilder.Append(']');
    }

    private static void WriteString(StringBuilder stringBuilder, string value)
    {
        stringBuilder.Append('\'');
        foreach (var c in value)
        {
            switch (c)
            {
                case '\'':
                    stringBuilder.Append("\\\'");
                    break;
                case '\\':
                    stringBuilder.Append("\\\\");
                    break;
                case '\b':
                    stringBuilder.Append("\\b");
                    break;
                case '\f':
                    stringBuilder.Append("\\f");
                    break;
                case '\n':
                    stringBuilder.Append("\\n");
                    break;
                case '\r':
                    stringBuilder.Append("\\r");
                    break;
                case '\t':
                    stringBuilder.Append("\\t");
                    break;
                default:
                    if (char.IsControl(c))
                    {
                        stringBuilder.Append("\\u");
                        stringBuilder.Append(((int)c).ToString("x4", CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        stringBuilder.Append(c);
                    }

                    break;
            }
        }

        stringBuilder.Append('\'');
    }
}
