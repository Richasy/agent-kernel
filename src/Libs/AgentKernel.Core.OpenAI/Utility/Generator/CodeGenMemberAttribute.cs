#nullable enable

using System;

namespace AgentKernel.Core.OpenAI;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
internal sealed class CodeGenMemberAttribute : CodeGenTypeAttribute
{
    public CodeGenMemberAttribute() : base(null)
    {
    }

    public CodeGenMemberAttribute(string originalName) : base(originalName)
    {
    }
}