// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Windows.Preview;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(int))]
internal sealed partial class JsonGenContext : JsonSerializerContext
{
}
