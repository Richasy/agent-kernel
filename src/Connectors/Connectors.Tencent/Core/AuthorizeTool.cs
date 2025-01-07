// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text;
using System.Security.Cryptography;
using System.Globalization;
using System.Net.Http.Headers;

namespace Richasy.AgentKernel.Connectors.Tencent.Core;

internal static class AuthorizeTool
{
    public static HttpRequestMessage CreateAuthorizedRequest(
        Uri endpoint,
        string payload,
        string secretId,
        string secretKey,
        string action,
        string version = "2018-03-21",
        string region = "ap-beijing")
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint);
        foreach (var kvp in BuildHeaders(endpoint, payload, secretId, secretKey, action, version, region))
        {
            if (kvp.Key.Equals("Content-Type", StringComparison.Ordinal))
            {
                ByteArrayContent content = new(Encoding.UTF8.GetBytes(payload));
                content.Headers.Remove("Content-Type");
                content.Headers.Add("Content-Type", kvp.Value);
                httpRequestMessage.Content = content;
            }
            else if (kvp.Key.Equals("Host", StringComparison.Ordinal))
            {
                httpRequestMessage.Headers.Host = kvp.Value;
            }
            else if (kvp.Key.Equals("Authorization", StringComparison.Ordinal))
            {
                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("TC3-HMAC-SHA256",
                    kvp.Value["TC3-HMAC-SHA256".Length..].Trim());
            }
            else
            {
                httpRequestMessage.Headers.TryAddWithoutValidation(kvp.Key, kvp.Value);
            }
        }

        return httpRequestMessage;
    }

    private static Dictionary<string, string> BuildHeaders(Uri endpoint, string payload, string secretId, string secretKey, string action, string version, string region)
    {
        const string httpRequestMethod = "POST";
        const string canonicalURI = "/";
        var canonicalHeaders = "content-type:application/json\nhost:" + endpoint.Host + $"\nx-tc-action:{action.ToLowerInvariant()}\n";
        const string signedHeaders = "content-type;host;x-tc-action";
        var hashedRequestPayload = SHA256Hex(payload);
        var canonicalRequest = httpRequestMethod + "\n"
                                                 + canonicalURI + "\n"
                                                 + string.Empty + "\n"
                                                 + canonicalHeaders + "\n"
                                                 + signedHeaders + "\n"
                                                 + hashedRequestPayload;

        const string algorithm = "TC3-HMAC-SHA256";
        var now = DateTimeOffset.Now;
        var timestamp = now.ToUnixTimeSeconds();
        var requestTimestamp = timestamp.ToString(CultureInfo.InvariantCulture);
        var date = now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var service = endpoint.Host.Split('.')[0];
        var credentialScope = date + "/" + service + "/" + "tc3_request";
        var hashedCanonicalRequest = SHA256Hex(canonicalRequest);
        var stringToSign = algorithm + "\n"
                                     + requestTimestamp + "\n"
                                     + credentialScope + "\n"
                                     + hashedCanonicalRequest;

        var tc3SecretKey = Encoding.UTF8.GetBytes("TC3" + secretKey);
        var secretDate = HmacSHA256(tc3SecretKey, Encoding.UTF8.GetBytes(date));
        var secretService = HmacSHA256(secretDate, Encoding.UTF8.GetBytes(service));
        var secretSigning = HmacSHA256(secretService, Encoding.UTF8.GetBytes("tc3_request"));
        var signatureBytes = HmacSHA256(secretSigning, Encoding.UTF8.GetBytes(stringToSign));
        var signature = Convert.ToHexStringLower(signatureBytes);

        var authorization = algorithm + " "
                                      + "Credential=" + secretId + "/" + credentialScope + ", "
                                      + "SignedHeaders=" + signedHeaders + ", "
                                      + "Signature=" + signature;

        return new()
        {
            { "Authorization", authorization },
            { "Host", endpoint.Host },
            { "Content-Type", "application/json" },
            { "X-TC-Timestamp", requestTimestamp },
            { "X-TC-Version", version },
            { "X-TC-Action", action },
            { "X-TC-Region", region }
        };
    }

    private static string SHA256Hex(string s)
        => SHA256Hex(Encoding.UTF8.GetBytes(s));

    private static string SHA256Hex(byte[] s)
    {
        var hashbytes = SHA256.HashData(s);
        StringBuilder builder = new();
        for (var i = 0; i < hashbytes.Length; ++i)
        {
            builder.Append(hashbytes[i].ToString("x2", CultureInfo.InvariantCulture));
        }

        return builder.ToString();
    }

    private static byte[] HmacSHA256(byte[] key, byte[] msg)
    {
        using HMACSHA256 mac = new(key);
        return mac.ComputeHash(msg);
    }
}
