// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Diagnostics.CodeAnalysis;

namespace Richasy.AgentKernel.Core.AzureOpenAI.Chat;

[Experimental("AOAI001")]
[CodeGenModel("AzureChatDataSourceEndpointVectorizationSource")]
internal partial class InternalAzureChatDataSourceEndpointVectorizationSource
{
    internal DataSourceAuthentication Authentication { get; set; }
}
