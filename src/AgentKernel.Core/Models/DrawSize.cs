
namespace Richasy.AgentKernel.Models;

/// <summary>
/// 绘制尺寸.
/// </summary>
public struct DrawSize(int width, int height) : IEquatable<DrawSize>
{
    /// <summary>
    /// 宽度.
    /// </summary>
    public int Width { get; set; } = width;

    /// <summary>
    /// 高度.
    /// </summary>
    public int Height { get; set; } = height;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is DrawSize size && Width == size.Width && Height == size.Height;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Width, Height);

    /// <summary>
    /// 比较两个对象是否相等.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(DrawSize left, DrawSize right) => left.Equals(right);

    /// <summary>
    /// 比较两个对象是否不相等.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator !=(DrawSize left, DrawSize right) => !(left == right);

    /// <inheritdoc/>
    public bool Equals(DrawSize other)
        => Width == other.Width && Height == other.Height;
}
