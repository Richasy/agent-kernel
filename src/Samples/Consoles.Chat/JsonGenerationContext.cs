// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Consoles.Chat;

[JsonSourceGenerationOptions]
[JsonSerializable(typeof(ChatConfiguration))]
internal sealed partial class JsonGenerationContext : JsonSerializerContext
{
}
