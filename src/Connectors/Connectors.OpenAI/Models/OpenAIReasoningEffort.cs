// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.OpenAI.Models;

/// <summary>
/// The reasoning effort for the OpenAI chat.
/// </summary>
[JsonConverter(typeof(OpenAIReasoningEffortConverter))]
public enum OpenAIReasoningEffort
{
    /// <summary>
    /// Low reasoning effort.
    /// </summary>
    Low,

    /// <summary>
    /// Medium reasoning effort.
    /// </summary>
    Medium,

    /// <summary>
    /// High reasoning effort.
    /// </summary>
    High,
}

/// <summary>
/// OpenAI reasoning effort converter.
/// </summary>
public sealed class OpenAIReasoningEffortConverter : JsonConverter<OpenAIReasoningEffort>
{
    /// <inheritdoc/>
    public override OpenAIReasoningEffort Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value switch
        {
            "low" => OpenAIReasoningEffort.Low,
            "medium" => OpenAIReasoningEffort.Medium,
            "high" => OpenAIReasoningEffort.High,
            _ => throw new JsonException(),
        };
    }
    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, OpenAIReasoningEffort value, JsonSerializerOptions options)
    {
        var text = value switch
        {
            OpenAIReasoningEffort.Low => "low",
            OpenAIReasoningEffort.Medium => "medium",
            OpenAIReasoningEffort.High => "high",
            _ => throw new JsonException(),
        };
        writer.WriteStringValue(text);
    }
}