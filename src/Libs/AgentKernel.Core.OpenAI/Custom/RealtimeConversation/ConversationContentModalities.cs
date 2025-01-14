using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AgentKernel.Core.OpenAI.RealtimeConversation;

[Experimental("OPENAI002")]
[Flags]
public enum ConversationContentModalities : int
{
    Text = 1 << 0,
    Audio = 1 << 1,
}