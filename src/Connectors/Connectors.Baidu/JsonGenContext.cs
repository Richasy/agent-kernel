// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.Baidu.Models;
using Richasy.AgentKernel.Connectors.Baidu.Models.Ernie;
using Richasy.AgentKernel.Connectors.Baidu.Models.Translation;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Baidu;

[JsonSourceGenerationOptions(WriteIndented = true, PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(BearerToken))]
[JsonSerializable(typeof(AccessToken))]
[JsonSerializable(typeof(ErnieChatRequest))]
[JsonSerializable(typeof(ErnieChatResponse))]
[JsonSerializable(typeof(ErnieErrorResponse))]
[JsonSerializable(typeof(ErnieBasicDrawRequest))]
[JsonSerializable(typeof(ErnieAdvancedDrawRequest))]
[JsonSerializable(typeof(ErnieDrawTaskResponse))]
[JsonSerializable(typeof(ErnieGetImageRequest))]
[JsonSerializable(typeof(ErnieBasicGetImageResponse))]
[JsonSerializable(typeof(ErnieAdvancedGetImageResponse))]
[JsonSerializable(typeof(OpenAIChatToolJson))]
[JsonSerializable(typeof(BaiduTranslateResponse))]
[JsonSerializable(typeof(ErnieWebSearchParameters))]
internal sealed partial class JsonGenContext : JsonSerializerContext
{
}
