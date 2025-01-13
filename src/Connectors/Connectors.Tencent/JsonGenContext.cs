// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Tencent.Models.Draw;
using Richasy.AgentKernel.Connectors.Tencent.Models.Translation;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Tencent;

[JsonSourceGenerationOptions(WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(TencentTranslateRequest))]
[JsonSerializable(typeof(TencentTranslateResponse))]
[JsonSerializable(typeof(HunyuanDrawCreateRequest))]
[JsonSerializable(typeof(HunyuanDrawQueryRequest))]
[JsonSerializable(typeof(HunyuanDrawCreateResponse))]
[JsonSerializable(typeof(HunyuanDrawQueryResponse))]
[JsonSerializable(typeof(HunyuanDrawLiteResponse))]
internal sealed partial class JsonGenContext : JsonSerializerContext
{
}
