using System;
using System.Diagnostics.CodeAnalysis;

namespace Richasy.AgentKernel.Core.OpenAI.RealtimeConversation;

[Experimental("OPENAI002")]
[CodeGenModel("RealtimeTurnDetectionType")]
public enum ConversationTurnDetectionKind
{
    [CodeGenMember("ServerVad")]
    ServerVoiceActivityDetection,
    Disabled,
}