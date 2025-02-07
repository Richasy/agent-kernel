// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel;
using System.Text.Json.Serialization;

namespace Consoles.Draw;

[JsonSourceGenerationOptions(DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(DrawClientConfiguration))]
internal sealed partial class JsonGenerationContext : JsonSerializerContext;
