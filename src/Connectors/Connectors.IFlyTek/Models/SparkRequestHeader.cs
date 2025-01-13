// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.IFlyTek.Models;

internal sealed class SparkRequestHeader
{
    /// <summary>
    /// The application appid, obtained from the open platform control panel.
    /// </summary>
    [JsonPropertyName("app_id")]
    public string? AppId { get; set; }

    /// <summary>
    /// The user's id, used to distinguish between different users. Not required.
    /// </summary>
    [JsonPropertyName("uid")]
    public string? Uid { get; set; }
}
