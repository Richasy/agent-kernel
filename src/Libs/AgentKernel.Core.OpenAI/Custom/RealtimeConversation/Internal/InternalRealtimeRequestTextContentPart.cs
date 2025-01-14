using System;
using System.Collections.Generic;

namespace AgentKernel.Core.OpenAI.RealtimeConversation;
internal partial class InternalRealtimeRequestTextContentPart : ConversationContentPart
{
    [CodeGenMember("Text")]
    public string InternalTextValue { get; set; }
}
