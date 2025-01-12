// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Mistral.Models;

internal sealed class MistralChatRequest
{
    public required string Model { get; set; }

    public required IList<MistralChatMessage> Messages { get; set; }

    public IList<MistralTool>? Tools { get; set; }

    public double? Temperature { get; set; }

    public double? TopP { get; set; }

    public int? MaxTokens { get; set; }

    public bool? Stream { get; set; }

    public string[]? Stop { get; set; }

    public int? RandomSeed { get; set; }

    public MistralResponseFormat? ResponseFormat { get; set; }

    public double? PresencePenalty { get; set; }

    public double? FrequencyPenalty { get; set; }

    public bool? SafePrompt { get; set; }
}
