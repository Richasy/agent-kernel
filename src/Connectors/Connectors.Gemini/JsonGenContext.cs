// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Gemini.Models.Core;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Gemini;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(GeminiRequest))]
[JsonSerializable(typeof(GeminiResponse))]
internal sealed partial class JsonGenContext : JsonSerializerContext
{
}
