using System.Diagnostics.CodeAnalysis;

namespace Richasy.AgentKernel.Core.OpenAI.Assistants;

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
