// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.IFlyTek.Models;
using Richasy.AgentKernel.Connectors.IFlyTek.Models.Draw;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.IFlyTek;

[JsonSourceGenerationOptions(WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(SparkDrawRequest))]
[JsonSerializable(typeof(SparkTextResponse))]
internal sealed partial class JsonGenContext : JsonSerializerContext;
