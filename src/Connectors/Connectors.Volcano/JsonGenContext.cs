// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Volcano.Models.Audio;
using Richasy.AgentKernel.Connectors.Volcano.Models.Translation;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Volcano;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(VolcanoTranslateRequest))]
[JsonSerializable(typeof(VolcanoTranslateResponse))]
[JsonSerializable(typeof(VolcanoAudioRequest))]
[JsonSerializable(typeof(VolcanoAudioResponse))]
internal sealed partial class JsonGenContext : JsonSerializerContext
{
}
