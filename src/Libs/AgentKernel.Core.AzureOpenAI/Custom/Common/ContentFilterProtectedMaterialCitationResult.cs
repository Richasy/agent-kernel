// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics.CodeAnalysis;

namespace AgentKernel.Core.AzureOpenAI;

[Experimental("AOAI001")]
[CodeGenModel("AzureContentFilterResultForChoiceProtectedMaterialCodeCitation")]
public partial class ContentFilterProtectedMaterialCitationResult
{
    // CUSTOM: Renamed for Uri type.
    [CodeGenMember("URL")]
    public Uri Uri { get; }
}