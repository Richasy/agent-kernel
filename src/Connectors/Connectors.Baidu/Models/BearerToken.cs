// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Baidu.Models;

/// <summary>
/// Baidu Bearer Token.
/// </summary>
internal sealed class BearerToken
{
    [JsonPropertyName("userId")]
    public required string UserId { get; set; }

    public required string Token { get; set; }

    public required string Status { get; set; }

    [JsonPropertyName("createTime")]
    public required DateTimeOffset CreateTime { get; set; }

    [JsonPropertyName("expireTime")]
    public required DateTimeOffset ExpireTime { get; set; }
}
