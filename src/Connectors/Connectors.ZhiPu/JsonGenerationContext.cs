// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Connectors.ZhiPu.Models;
using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.ZhiPu;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(ZhiPuChatRequest))]
[JsonSerializable(typeof(ZhiPuChatResponse))]
[JsonSerializable(typeof(ZhiPuBasicChatRequest))]
[JsonSerializable(typeof(ZhiPuContentChatRequest))]
[JsonSerializable(typeof(ZhiPuChatRequestBasicMessage))]
[JsonSerializable(typeof(ZhiPuChatRequestAssistantMessage))]
[JsonSerializable(typeof(ZhiPuChatRequestContentMessage))]
[JsonSerializable(typeof(ZhiPuChatRequestToolMessage))]
[JsonSerializable(typeof(ZhiPuErrorResponse))]
internal sealed partial class JsonGenerationContext : JsonSerializerContext;
