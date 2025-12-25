// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Azure.Models.Audio;
using Richasy.AgentKernel.Connectors.Azure.Models.Translation;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Azure;

[JsonSourceGenerationOptions(WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(List<TranslationTextItem>))]
[JsonSerializable(typeof(List<TranslationResponse>))]
[JsonSerializable(typeof(List<EdgeVoice>))]
[JsonSerializable(typeof(List<AzureVoice>))]
[JsonSerializable(typeof(EdgeSpeechConfig))]
internal sealed partial class JsonGenContext : JsonSerializerContext
{
}
