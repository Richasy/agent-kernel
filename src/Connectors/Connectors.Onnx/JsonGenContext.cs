// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Onnx.Models;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Onnx;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(ModelTemplate))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(bool))]
internal sealed partial class JsonGenContext : JsonSerializerContext
{
}
