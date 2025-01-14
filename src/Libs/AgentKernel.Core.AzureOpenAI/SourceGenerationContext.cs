using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Richasy.AgentKernel.Core.AzureOpenAI;

[JsonSourceGenerationOptions]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(int?))]
[JsonSerializable(typeof(JsonElement))]
[JsonSerializable(typeof(IEnumerable<string>))]
[JsonSerializable(typeof(IEnumerable<ReadOnlyMemory<int>>))]
[JsonSerializable(typeof(ReadOnlyMemory<float>))]
internal sealed partial class SourceGenerationContext : JsonSerializerContext;
