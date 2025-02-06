// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Models;

/// <summary>
/// 音频模型.
/// </summary>
public sealed class AudioModel
{
    /// <summary>
    /// 标识符.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// 显示名称.
    /// </summary>
    [JsonPropertyName("name")]
    public string? DisplayName { get; set; }

    /// <summary>
    /// 声音列表.
    /// </summary>
    [JsonPropertyName("voices")]
    public required IList<AudioVoice> Voices { get; set; }

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is AudioModel model && Id == model.Id;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Id);

    /// <inheritdoc/>
    public override string ToString() => DisplayName ?? Id;
}
