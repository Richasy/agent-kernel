// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Globalization;
using System.Text.RegularExpressions;

namespace Richasy.AgentKernel.Connectors.Google.Core;

/// <summary>
/// Acquire Google Translate token.
/// </summary>
public sealed class GoogleTranslateTokenAcquirer(HttpClient client, string tkk = "0", string host = "https://translate.google.com")
{
    private static readonly Regex RE_TKK = new("tkk:'(.+?)'", RegexOptions.Singleline);
    private readonly string host = host.Contains("http", StringComparison.InvariantCultureIgnoreCase) ? host : "https://" + host;

    /// <summary>
    /// Generate token.
    /// </summary>
    /// <returns>Token.</returns>
    public async Task<string> GenerateAsync(string text, CancellationToken cancellationToken = default)
    {
        await UpdateAsync(cancellationToken).ConfigureAwait(false);
        return Acquire(text);
    }

    private async Task UpdateAsync(CancellationToken cancellationToken = default)
    {
        var now = Math.Floor(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() / 3600000.0);
        if (!string.IsNullOrEmpty(tkk) && int.Parse(tkk.Split('.')[0], CultureInfo.InvariantCulture) == now)
        {
            return;
        }

        var response = await client.GetAsync(new Uri(host), cancellationToken).ConfigureAwait(false);
        var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        var rawTkkMatch = RE_TKK.Match(content);

        if (rawTkkMatch.Success)
        {
            tkk = rawTkkMatch.Groups[1].Value;
        }
    }

    private static int Xr(int a, string b)
    {
        var size_b = b.Length;
        var c = 0;
        while (c < size_b - 2)
        {
            int d = b[c + 2];
            d = char.IsLetter((char)d) ? d - 87 : int.Parse(d.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
            d = b[c + 1] == '+' ? a >> d : a << d;
            a = (int)(b[c] == '+' ? (a + d) & 4294967295 : a ^ d);
            c += 3;
        }

        return a;
    }

    private string Acquire(string text)
    {
        var a = new List<int>();
        foreach (var ch in text)
        {
            int val = ch;
            if (val < 0x10000)
            {
                a.Add(val);
            }
            else
            {
                a.Add((int)Math.Floor((double)((val - 0x10000) / 0x400) + 0xD800));
                a.Add((int)Math.Floor((double)((val - 0x10000) % 0x400) + 0xDC00));
            }
        }

        var b = tkk != "0" ? tkk : "";
        var d = b.Split('.');
        var bVal = d.Length > 1 ? int.Parse(d[0], CultureInfo.InvariantCulture) : 0;

        var e = new List<int>();
        for (var g = 0; g < a.Count; g++)
        {
            var l = a[g];
            if (l < 128)
            {
                e.Add(l);
            }
            else
            {
                if (l < 2048)
                {
                    e.Add((l >> 6) | 192);
                }
                else
                {
                    if ((l & 64512) == 55296 && g + 1 < a.Count && (a[g + 1] & 64512) == 56320)
                    {
                        g++;
                        l = 65536 + ((l & 1023) << 10) + (a[g] & 1023);
                        e.Add((l >> 18) | 240);
                        e.Add(((l >> 12) & 63) | 128);
                    }
                    else
                    {
                        e.Add((l >> 12) | 224);
                    }

                    e.Add(((l >> 6) & 63) | 128);
                }

                e.Add((l & 63) | 128);
            }
        }

        var aVal = bVal;
        foreach (var value in e)
        {
            aVal += value;
            aVal = Xr(aVal, "+-a^+6");
        }

        aVal = Xr(aVal, "+-3^+b+-f");
        aVal ^= d.Length > 1 ? int.Parse(d[1], CultureInfo.InvariantCulture) : 0;
        if (aVal < 0)
        {
            aVal = (int)((aVal & 2147483647) + 2147483648);
        }

        aVal %= 1000000;

        return $"{aVal}.{aVal ^ bVal}";
    }
}
