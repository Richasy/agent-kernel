using System.Diagnostics.CodeAnalysis;

namespace AgentKernel.Core.OpenAI.RealtimeConversation;

[Experimental("OPENAI002")]
[CodeGenModel("RealtimeResponseMessageItem")]
internal partial class InternalRealtimeResponseMessageItem
{
    [CodeGenMember("Role")]
    public ConversationMessageRole Role { get; }
}
