// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Baidu.Models;

internal sealed class ErnieChatRequest
{
    public required string Model { get; set; }

    public required IList<ErnieChatMessage> Messages { get; set; }

    public bool? Stream { get; set; }

    public double? Temperature { get; set; }

    public double? TopP { get; set; }

    public double? PenaltyScore { get; set; }

    public int? MaxCompletionTokens { get; set; }

    public long? Seed { get; set; }

    public string? User { get; set; }

    public IList<string>? Stop { get; set; }

    public double? FrequencyPenalty { get; set; }

    public double? PresencePenalty { get; set; }

    public string? ToolChoice { get; set; }

    public bool? ParallelToolCalls { get; set; }

    public ErnieWebSearchTool? WebSearch { get; set; }

    public IList<ErnieTool>? Tools { get; set; }

    public ErnieResponseFormat? ResponseFormat { get; set; }
}
