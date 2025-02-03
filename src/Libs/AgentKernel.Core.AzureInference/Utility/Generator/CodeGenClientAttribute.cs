// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#nullable enable

namespace Richasy.AgentKernel.Core.AzureInference;

[AttributeUsage(AttributeTargets.Class)]
internal class CodeGenClientAttribute : CodeGenTypeAttribute
{
    public Type? ParentClient { get; set; }

    public CodeGenClientAttribute(string originalName) : base(originalName)
    {
    }
}
