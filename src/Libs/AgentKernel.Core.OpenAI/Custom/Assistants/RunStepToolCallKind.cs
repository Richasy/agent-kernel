using System.Diagnostics.CodeAnalysis;

namespace Richasy.AgentKernel.Core.OpenAI.Assistants;

[Experimental("OPENAI001")]
[CodeGenModel("RunStepDetailsToolCallKind")]
public enum RunStepToolCallKind
{
    CodeInterpreter,
    FileSearch,
    Function,
}