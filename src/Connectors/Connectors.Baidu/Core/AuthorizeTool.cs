// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Baidu.Models;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Richasy.AgentKernel.Connectors.Baidu.Core;

internal static class AuthorizeTool
{
    public static async Task<BearerToken?> GenerateBearerTokenAsync(string accessKey, string secretKey, CancellationToken cancellationToken = default)
    {
        var authorization = GenerateAuthorizeString(secretKey, accessKey);
        var request = new HttpRequestMessage(HttpMethod.Get, "https://iam.bj.baidubce.com/v1/BCE-BEARER/token?expireInSeconds=3600");
        request.Headers.TryAddWithoutValidation("Authorization", authorization);
        using var client = new HttpClient();
        var response = await client.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        return JsonSerializer.Deserialize(content, JsonGenContext.Default.BearerToken);
    }

    public static async Task<AccessToken?> GenerateAccessTokenAsync(string accessKey, string secretKey, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://aip.baidubce.com/oauth/2.0/token");
        var body = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", accessKey },
            { "client_secret", secretKey },
        };

        request.Content = new FormUrlEncodedContent(body);
        using var client = new HttpClient();
        var response = await client.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        return JsonSerializer.Deserialize(content, JsonGenContext.Default.AccessToken);
    }

    private static string GenerateAuthorizeString(string sk, string ak)
    {
        var timestamp = DateTimeOffset.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
        const string expirationPeriodInSeconds = "3600"; // 设置签名有效期为 1800 秒

        // 任务一：创建前缀字符串(authStringPrefix)
        var authStringPrefix = $"bce-auth-v1/{ak}/{timestamp}/{expirationPeriodInSeconds}";

        // 任务二：创建规范请求(canonicalRequest)
        // 这里的值根据实际情况进行设置，示例中使用了假定的值
        const string httpMethod = "GET";
        const string canonicalURI = "/v1/BCE-BEARER/token";
        const string canonicalHeaders = "host:iam.bj.baidubce.com";
        var canonicalQueryString = $"expireInSeconds={expirationPeriodInSeconds}";
        var canonicalRequest = $"{httpMethod}\n{canonicalURI}\n{canonicalQueryString}\n{canonicalHeaders}";

        // 任务三：生成派生签名密钥(signingKey)
        var signingKey = HmacSha256Hex(sk, authStringPrefix);

        // 任务四：生成签名摘要(signature)
        var signature = HmacSha256Hex(signingKey, canonicalRequest);

        // 最终的认证字符串(authorization)
        return $"{authStringPrefix}/host/{signature}";
    }

    private static string HmacSha256Hex(string key, string data)
    {
        using var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(key));
        var hashBytes = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(data));
        return Convert.ToHexStringLower(hashBytes);
    }
}
