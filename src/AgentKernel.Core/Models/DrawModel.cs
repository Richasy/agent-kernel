using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Models;

/// <summary>
/// 绘图模型.
/// </summary>
public sealed class DrawModel
{
    /// <summary>
    /// 初始化 <see cref="DrawModel"/> 类的新实例.
    /// </summary>
    public DrawModel()
    {
        Id = string.Empty;
        Name = string.Empty;
        SupportSizes = [];
    }

    /// <summary>
    /// 初始化 <see cref="DrawModel"/> 类的新实例.
    /// </summary>
    public DrawModel(string id, string name, bool negativeSupport, params DrawSize[] sizes)
    {
        Id = id;
        Name = name;
        IsNegativeSupport = negativeSupport;
        SupportSizes = sizes;
    }

    /// <summary>
    /// 获取或设置模型 ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    /// 获取或设置模型的显示名称.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// 获取或设置该模型是否支持负提示词.
    /// </summary>
    [JsonPropertyName("negative_support")]
    public bool IsNegativeSupport { get; set; }

    /// <summary>
    /// 支持的尺寸.
    /// </summary>
    [JsonPropertyName("sizes")]
    public IReadOnlyList<DrawSize> SupportSizes { get; set; }

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is DrawModel model && Id == model.Id;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Id);

    /// <inheritdoc/>
    public override string ToString() => Name ?? Id;
}
