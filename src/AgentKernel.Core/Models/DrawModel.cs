using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Models;

/// <summary>
/// 绘图模型.
/// </summary>
public sealed class DrawModel(string id, string name, bool negativeSupport, params DrawSize[] sizes)
{
    /// <summary>
    /// 获取或设置模型 ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = id;

    /// <summary>
    /// 获取或设置模型的显示名称.
    /// </summary>
    [JsonPropertyName("name")]
    public string? DisplayName { get; set; } = name;

    /// <summary>
    /// 获取或设置该模型是否支持负提示词.
    /// </summary>
    [JsonPropertyName("negative_support")]
    public bool IsNegativeSupport { get; set; } = negativeSupport;

    /// <summary>
    /// 支持的尺寸.
    /// </summary>
    [JsonPropertyName("sizes")]
    public IReadOnlyList<DrawSize> SupportSizes { get; set; } = sizes;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is DrawModel model && Id == model.Id;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Id);

    /// <inheritdoc/>
    public override string ToString() => DisplayName ?? Id;
}
