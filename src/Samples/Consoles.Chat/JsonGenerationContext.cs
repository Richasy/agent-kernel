// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Consoles.Chat;

[JsonSourceGenerationOptions(DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(ChatConfiguration))]
[JsonSerializable(typeof(string))]
internal sealed partial class JsonGenerationContext : JsonSerializerContext
{
}
