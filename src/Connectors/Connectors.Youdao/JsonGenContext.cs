// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Youdao.Models.Translation;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Youdao;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(YoudaoTranslateResult))]
internal sealed partial class JsonGenContext : JsonSerializerContext
{
}
