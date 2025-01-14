using System.Diagnostics.CodeAnalysis;

namespace Richasy.AgentKernel.Core.OpenAI.Assistants;

/// <summary>
/// Represents additional options available when modifying an existing <see cref="ThreadRun"/>.
/// </summary>
[Experimental("OPENAI001")]
[CodeGenModel("ModifyRunRequest")]
public partial class RunModificationOptions
{
}
