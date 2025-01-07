// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Tencent.Models.Translation;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Tencent;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(TencentTranslateRequest))]
[JsonSerializable(typeof(TencentTranslateResponse))]
internal sealed partial class JsonGenContext : JsonSerializerContext
{
}
