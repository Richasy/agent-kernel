using System;

namespace AgentKernel.Core.OpenAI.Chat;

// CUSTOM: Renamed.
[CodeGenModel("ChatCompletionMessageToolCallChunkFunction")]
[CodeGenSerialization(nameof(Arguments), SerializationValueHook = nameof(SerializeArgumentsValue), DeserializationValueHook = nameof(DeserializeArgumentsValue))]
internal partial class InternalChatCompletionMessageToolCallChunkFunction
{
    // CUSTOM: Changed type from string to BinaryData.
    public BinaryData Arguments { get; }
}