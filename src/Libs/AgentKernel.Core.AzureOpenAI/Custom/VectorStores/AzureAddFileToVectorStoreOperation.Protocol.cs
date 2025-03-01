// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#if !AZURE_OPENAI_GA

using System.ClientModel.Primitives;

namespace Richasy.AgentKernel.Core.AzureOpenAI.VectorStores;
internal partial class AzureAddFileToVectorStoreOperation : AddFileToVectorStoreOperation
{
    internal override PipelineMessage CreateGetVectorStoreFileRequest(string vectorStoreId, string fileId, RequestOptions options)
        => new AzureOpenAIPipelineMessageBuilder(_pipeline, _endpoint, _apiVersion)
            .WithMethod("GET")
            .WithPath("vector_stores", vectorStoreId, "files", fileId)
            .WithAccept("application/json")
            .WithOptions(options)
            .Build();
}

#endif