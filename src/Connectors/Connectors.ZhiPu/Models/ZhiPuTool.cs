// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json;

namespace Richasy.AgentKernel.Connectors.ZhiPu.Models;

internal sealed class ZhiPuTool
{
    public required string Type { get; set; }

    public ZhiPuFunctionTool? Function { get; set; }

    public ZhiPuRetrievalTool? Retrieval { get; set; }

    public ZhiPuWebSearchTool? WebSearch { get; set; }

    public static ZhiPuTool CreateFunctionTool(string name, string description, IDictionary<string, JsonElement> properties, string[] required)
    {
        return new ZhiPuTool
        {
            Type = "function",
            Function = new ZhiPuFunctionTool
            {
                Name = name,
                Description = description,
                Parameters = new ZhiPuFunctionToolParameters
                {
                    Properties = properties,
                    Required = required,
                },
            },
        };
    }

    public static ZhiPuTool CreateRetrievalTool(string knowledgeId, string promptTemplate)
    {
        return new ZhiPuTool
        {
            Type = "retrieval",
            Retrieval = new ZhiPuRetrievalTool
            {
                KnowledgeId = knowledgeId,
                PromptTemplate = promptTemplate,
            },
        };
    }

    public static ZhiPuTool CreateWebSearchTool(string? query, bool showSearchResult = false)
    {
        var tool = new ZhiPuTool
        {
            Type = "web_search",
            WebSearch = new ZhiPuWebSearchTool
            {
                Enable = true,
                SearchResult = showSearchResult,
            }
        };

        if (!string.IsNullOrEmpty(query))
        {
            tool.WebSearch.SearchQuery = query;
        }

        return tool;
    }
}
