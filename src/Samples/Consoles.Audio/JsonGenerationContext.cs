// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel;
using System.Text.Json.Serialization;

namespace Consoles.Audio;

[JsonSourceGenerationOptions(DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(AudioClientConfiguration))]
internal sealed partial class JsonGenerationContext : JsonSerializerContext;
