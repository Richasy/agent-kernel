using System.Diagnostics.CodeAnalysis;

namespace AgentKernel.Core.OpenAI.Assistants;

/// <summary>
/// Represents additional options available when modifying an existing <see cref="ThreadMessage"/>.
/// </summary>
[Experimental("OPENAI001")]
[CodeGenModel("ModifyMessageRequest")]
public partial class MessageModificationOptions
{
}
