// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Baidu.Models;

internal sealed class AccessToken
{
    [JsonPropertyName("access_token")]
    public string? Token { get; set; }

    public int? ExpiresIn { get; set; }
}
