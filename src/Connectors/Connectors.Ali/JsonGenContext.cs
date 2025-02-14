// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Ali.Models.Translation;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Ali;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(AliTranslateRequest))]
[JsonSerializable(typeof(AliTranslateResponse))]
[JsonSerializable(typeof(bool))]
internal sealed partial class JsonGenContext : JsonSerializerContext
{
}
