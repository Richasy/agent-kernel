// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Models;

/// <summary>
/// 音频声音.
/// </summary>
public sealed class AudioVoice
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AudioVoice"/> class.
    /// </summary>
    public AudioVoice(
        string id,
        string name,
        VoiceGender gender,
        params string[] languages)
    {
        Id = id;
        DisplayName = name;
        Gender = gender;
        Languages = [.. languages];
    }

    /// <summary>
    /// 标识符.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    /// 声音显示名称.
    /// </summary>
    [JsonPropertyName("name")]
    public string DisplayName { get; set; }

    /// <summary>
    /// 声音性别.
    /// </summary>
    [JsonPropertyName("gender")]
    public VoiceGender Gender { get; set; }

    /// <summary>
    /// 支持的语言列表.
    /// </summary>
    [JsonPropertyName("languages")]
    public IList<string> Languages { get; set; }

    /// <summary>
    /// 解码格式.
    /// </summary>
    public string? Codec { get; set; }

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is AudioVoice voice && Id == voice.Id;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Id);
}
