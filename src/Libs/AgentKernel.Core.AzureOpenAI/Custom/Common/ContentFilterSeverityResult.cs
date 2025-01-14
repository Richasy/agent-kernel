// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Diagnostics.CodeAnalysis;

namespace AgentKernel.Core.AzureOpenAI;

[Experimental("AOAI001")]
[CodeGenModel("AzureContentFilterSeverityResult")]
public partial class ContentFilterSeverityResult
{
    [CodeGenMember("Severity")]
    public ContentFilterSeverity Severity { get; }
}