// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Core.AzureOpenAI.Internal;
using Richasy.AgentKernel.Core.OpenAI.Images;
using System.Diagnostics.CodeAnalysis;

namespace Richasy.AgentKernel.Core.AzureOpenAI.Images;

[Experimental("AOAI001")]
public static class AzureImageExtensions
{
    [Experimental("AOAI001")]
    public static RequestImageContentFilterResult GetRequestContentFilterResult(this GeneratedImage image)
    {
        return AdditionalPropertyHelpers.GetAdditionalPropertyAsRequestImageContentFilterResult(
            image.SerializedAdditionalRawData,
            "prompt_filter_results");
    }

    [Experimental("AOAI001")]
    public static ResponseImageContentFilterResult GetResponseContentFilterResult(this GeneratedImage image)
    {
        return AdditionalPropertyHelpers.GetAdditionalPropertyAsResponseImageContentFilterResult(
            image.SerializedAdditionalRawData,
            "content_filter_results");
    }
}
