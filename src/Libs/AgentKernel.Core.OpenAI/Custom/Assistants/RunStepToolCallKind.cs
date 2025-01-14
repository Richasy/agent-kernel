using System.Diagnostics.CodeAnalysis;

namespace AgentKernel.Core.OpenAI.Assistants;

[Experimental("OPENAI001")]
[CodeGenModel("RunStepDetailsToolCallKind")]
public enum RunStepToolCallKind
{
    CodeInterpreter,
    FileSearch,
    Function,
}