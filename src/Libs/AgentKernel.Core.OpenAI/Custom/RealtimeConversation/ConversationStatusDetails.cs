using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Richasy.AgentKernel.Core.OpenAI.RealtimeConversation;

[Experimental("OPENAI002")]
[CodeGenModel("RealtimeResponseStatusDetails")]
public partial class ConversationStatusDetails
{
    [CodeGenMember("Kind")]
    public ConversationStatus StatusKind { get; }
}