// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.OpenAI.Models;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.OpenAI;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(OpenAIReasoningEffort))]
[JsonSerializable(typeof(bool))]
internal sealed partial class JsonGenContext : JsonSerializerContext
{
}
