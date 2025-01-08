// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Google.Models.Core;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Google;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(GeminiRequest))]
[JsonSerializable(typeof(GeminiResponse))]
internal sealed partial class JsonGenContext : JsonSerializerContext
{
}
