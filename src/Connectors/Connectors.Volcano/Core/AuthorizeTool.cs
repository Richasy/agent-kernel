// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace Richasy.AgentKernel.Connectors.Volcano.Core;

internal static class AuthorizeTool
{
    public static HttpRequestMessage CreateAuthorizedRequest(Uri endpoint, string payload, string secretId, string secretKey, string region = "cn-north-1")
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, endpoint);
        var headers = BuildHeaders(endpoint, payload, secretId, secretKey, region);
        foreach (var kvp in headers)
        {
            if (kvp.Key.Equals("Authorization", StringComparison.Ordinal))
            {
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("HMAC-SHA256",
                    kvp.Value.Substring("HMAC-SHA256".Length + 1));
            }
            else if (kvp.Key.Equals("Host", StringComparison.Ordinal))
            {
                httpRequest.Headers.Host = kvp.Value;
            }
            else
            {
                httpRequest.Headers.TryAddWithoutValidation(kvp.Key, kvp.Value);
            }
        }

        httpRequest.Content = new StringContent(payload, Encoding.UTF8, "application/json");
        return httpRequest;
    }

    private static Dictionary<string, string> BuildHeaders(Uri endpoint, string payload, string secretId, string secretKey, string region)
    {
        var query = endpoint.Query.TrimStart('?');
        var now = DateTime.UtcNow;
        var dateFull = now.ToString("yyyyMMddTHHmmssZ", CultureInfo.InvariantCulture);
        var date = now.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
        const string httpRequestMethod = "POST";
        const string canonicalURI = "/";
        var canonicalHeaders = "host:" + endpoint.Host + $"\nx-date:{dateFull}\n";
        const string signedHeaders = "host;x-date";
        var hashedRequestPayload = SHA256Hex(payload);
        var canonicalRequest = httpRequestMethod + "\n"
                                                    + canonicalURI + "\n"
                                                    + query + "\n"
                                                    + canonicalHeaders + "\n"
                                                    + signedHeaders + "\n"
                                                    + hashedRequestPayload;
        const string algorithm = "HMAC-SHA256";
        var service = endpoint.Host.Split('.')[0];
        var credentialScope = date + "/" + region + "/" + service + "/" + "request";
        var hashedCanonicalRequest = SHA256Hex(canonicalRequest);
        var stringToSign = algorithm + "\n"
                                        + dateFull + "\n"
                                        + credentialScope + "\n"
                                        + hashedCanonicalRequest;

        var secretKeyHash = Encoding.UTF8.GetBytes(secretKey);
        var secretDate = HmacSHA256(secretKeyHash, Encoding.UTF8.GetBytes(date));
        var secretRegion = HmacSHA256(secretDate, Encoding.UTF8.GetBytes(region));
        var secretService = HmacSHA256(secretRegion, Encoding.UTF8.GetBytes(service));
        var secretSigning = HmacSHA256(secretService, Encoding.UTF8.GetBytes("request"));
        var signatureBytes = HmacSHA256(secretSigning, Encoding.UTF8.GetBytes(stringToSign));
        var signature = HexEncode(signatureBytes);

        var authorization = algorithm + " "
                                         + "Credential=" + secretId + "/" + credentialScope + ", "
                                         + "SignedHeaders=" + signedHeaders + ", "
                                         + "Signature=" + signature;

        return new()
        {
            { "Authorization", authorization },
            { "Host", endpoint.Host },
            { "X-Date", dateFull }
        };
    }

    private static string SHA256Hex(string s)
        => SHA256Hex(Encoding.UTF8.GetBytes(s));

    private static string SHA256Hex(byte[] s)
    {
        var hashbytes = SHA256.HashData(s);
        return HexEncode(hashbytes);
    }

    private static string HexEncode(byte[] s)
    {
        StringBuilder builder = new();
        for (var i = 0; i < s.Length; ++i)
        {
            builder.Append(s[i].ToString("x2", CultureInfo.InvariantCulture));
        }

        return builder.ToString();
    }

    private static byte[] HmacSHA256(byte[] key, byte[] msg)
    {
        using HMACSHA256 mac = new(key);
        return mac.ComputeHash(msg);
    }
}
