// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.OpenAI;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(bool))]
internal sealed partial class JsonGenContext : JsonSerializerContext
{
}
