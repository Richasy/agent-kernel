// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel;
using System.Text.Json.Serialization;

namespace Consoles.Chat;

[JsonSourceGenerationOptions(DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(ChatClientConfiguration))]
[JsonSerializable(typeof(string))]
internal sealed partial class JsonGenerationContext : JsonSerializerContext
{
}
