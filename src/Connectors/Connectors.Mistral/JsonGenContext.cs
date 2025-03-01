// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Mistral.Models;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Mistral;

[JsonSourceGenerationOptions(WriteIndented = true, PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(MistralChatRequest))]
[JsonSerializable(typeof(MistralChatResponse))]
[JsonSerializable(typeof(MistralErrorResponse))]
[JsonSerializable(typeof(MistralFunctionToolParameters))]
[JsonSerializable(typeof(string))]
internal sealed partial class JsonGenContext : JsonSerializerContext;
