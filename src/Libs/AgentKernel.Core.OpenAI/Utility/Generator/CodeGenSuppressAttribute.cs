using System;

namespace AgentKernel.Core.OpenAI;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Struct, AllowMultiple = true)]
internal sealed class CodeGenSuppressAttribute : Attribute
{
    public string Member { get; }
    public Type[] Parameters { get; }

    public CodeGenSuppressAttribute(string member, params Type[] parameters)
    {
        Member = member;
        Parameters = parameters;
    }
}