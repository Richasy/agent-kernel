#nullable enable

using System;

namespace AgentKernel.Core.OpenAI;

[AttributeUsage(AttributeTargets.Class)]
internal class CodeGenTypeAttribute : Attribute
{
    public string? OriginalName { get; }

    public CodeGenTypeAttribute(string? originalName)
    {
        OriginalName = originalName;
    }
}