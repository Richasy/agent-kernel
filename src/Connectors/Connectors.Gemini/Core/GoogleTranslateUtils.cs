// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json;
using System.Text.RegularExpressions;

namespace Richasy.AgentKernel.Connectors.Google.Core;

internal static class GoogleTranslateUtils
{
    internal static readonly string[] dt_values = ["at", "bd", "ex", "ld", "md", "qca", "rw", "rm", "ss", "t"];

    public static Dictionary<string, object> BuildParams(
        string client,
        string query,
        string src,
        string dest,
        string token,
        Dictionary<string, object>? overrideParams = null)
    {
        var @params = new Dictionary<string, object>
        {
            { "client", client },
            { "sl", src },
            { "tl", dest },
            { "hl", dest },
            { "dt", dt_values },
            { "ie", "UTF-8" },
            { "oe", "UTF-8" },
            { "otf", 1 },
            { "ssel", 0 },
            { "tsel", 0 },
            { "tk", token },
            { "q", query }
        };

        if (overrideParams != null)
        {
            foreach (var kvp in overrideParams)
            {
                @params[kvp.Key] = kvp.Value;
            }
        }

        return @params;
    }

    public static BinaryData LegacyFormatJson(string original)
    {
        var states = new List<Tuple<int, string>>();
        var text = original;

        // Save position for double-quoted texts
        var matches = Regex.Matches(text, "\"");
        for (var i = 0; i < matches.Count; i++)
        {
            var pos = matches[i].Index + 1;
            if (i % 2 == 0)
            {
                var nxt = text.IndexOf('"', pos);
                states.Add(new Tuple<int, string>(pos, text[pos..nxt]));
            }
        }

        // Replace all weird characters in text
        while (text.Contains(",,", StringComparison.InvariantCultureIgnoreCase))
        {
            text = text.Replace(",,", ",null,", StringComparison.InvariantCultureIgnoreCase);
        }

        while (text.Contains("[,", StringComparison.InvariantCultureIgnoreCase))
        {
            text = text.Replace("[,", "[null,", StringComparison.InvariantCultureIgnoreCase);
        }

        // Recover state
        matches = Regex.Matches(text, "\"");
        for (var i = 0; i < matches.Count; i++)
        {
            var pos = matches[i].Index + 1;
            if (i % 2 == 0)
            {
                var j = i / 2;
                var nxt = text.IndexOf('"', pos);
                text = string.Concat(text.AsSpan(0, pos), states[j].Item2, text.AsSpan(nxt));
            }
        }

        return BinaryData.FromString(text);
    }

    public static BinaryData FormatJson(string original)
    {
        BinaryData converted;
        try
        {
            converted = BinaryData.FromString(original);
        }
        catch (JsonException)
        {
            converted = LegacyFormatJson(original);
        }

        return converted;
    }

    public static int RShift(int val, int n) => (int)((uint)val >> n);
}
