using System.Diagnostics.CodeAnalysis;

namespace AgentKernel.Core.OpenAI.Assistants;

[Experimental("OPENAI001")]
[CodeGenModel("RunStepObjectType")]
public enum RunStepKind
{
    // CUSTOM: Renamed.
    [CodeGenMember("MessageCreation")]
    CreatedMessage,

    // CUSTOM: Renamed.
    [CodeGenMember("ToolCalls")]
    ToolCall,
}
