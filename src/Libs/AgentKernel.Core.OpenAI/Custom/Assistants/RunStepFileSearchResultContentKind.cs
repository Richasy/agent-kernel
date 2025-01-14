using System.Diagnostics.CodeAnalysis;

namespace AgentKernel.Core.OpenAI.Assistants;

// CUSTOM: Renamed.
[Experimental("OPENAI001")]
[CodeGenModel("RunStepDetailsToolCallsFileSearchResultObjectContentType")]
public enum RunStepFileSearchResultContentKind
{
    Text,
}