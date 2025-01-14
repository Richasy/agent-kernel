using System;
using System.Collections.Generic;

namespace AgentKernel.Core.OpenAI.RealtimeConversation;

internal partial class InternalRealtimeResponseTextContentPart : ConversationContentPart
{
    [CodeGenMember("Text")]
    public string InternalTextValue { get; }
}
