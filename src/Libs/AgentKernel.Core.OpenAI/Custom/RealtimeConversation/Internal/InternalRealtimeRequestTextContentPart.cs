using System;
using System.Collections.Generic;

namespace Richasy.AgentKernel.Core.OpenAI.RealtimeConversation;
internal partial class InternalRealtimeRequestTextContentPart : ConversationContentPart
{
    [CodeGenMember("Text")]
    public string InternalTextValue { get; set; }
}
