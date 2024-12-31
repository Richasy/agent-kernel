namespace Richasy.AgentKernel.Connectors.Mistral.Models;

/// <summary>
/// Mistral configuration.
/// </summary>
public sealed class MistralServiceConfig(string key, string model) : AIServiceConfig(key, model)
{
}
