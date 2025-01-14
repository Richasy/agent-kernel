// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using AgentKernel.Core.AzureOpenAI.Internal;
using System.Diagnostics.CodeAnalysis;

namespace AgentKernel.Core.AzureOpenAI.Images;

[Experimental("AOAI001")]
public static class AzureImageExtensions
{
    [Experimental("AOAI001")]
    public static RequestImageContentFilterResult GetRequestContentFilterResult(this GeneratedImage image)
    {
        return AdditionalPropertyHelpers.GetAdditionalProperty<RequestImageContentFilterResult>(
            image.SerializedAdditionalRawData,
            "prompt_filter_results");
    }

    [Experimental("AOAI001")]
    public static ResponseImageContentFilterResult GetResponseContentFilterResult(this GeneratedImage image)
    {
        return AdditionalPropertyHelpers.GetAdditionalProperty<ResponseImageContentFilterResult>(
            image.SerializedAdditionalRawData,
            "content_filter_results");
    }
}
