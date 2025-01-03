
namespace Richasy.AgentKernel.Connectors.Mistral.Models;

/// <summary>
/// Mistral configuration.
/// </summary>
public sealed class MistralServiceConfig(string key, string model, bool useCodestralApi) : AIServiceConfig(key, model)
{
    /// <summary>
    /// 是否使用 Codestral API.
    /// </summary>
    public bool UseCodestralApi { get; set; } = useCodestralApi;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is MistralServiceConfig config && base.Equals(obj) && AccessKey == config.AccessKey && Model == config.Model;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), AccessKey, Model);
}
