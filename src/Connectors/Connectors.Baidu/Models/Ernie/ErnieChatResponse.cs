// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Connectors.Baidu.Models;

internal sealed class ErnieChatResponse
{
    public required string Id { get; set; }

    public required string Object { get; set; }

    public long Created { get; set; }

    public string? Model { get; set; }

    public IList<ErnieChatResponseChoice>? Choices { get; set; }

    public ErnieChatUsage? Usage { get; set; }

    public IList<ErnieWebSearchResult>? SearchResults { get; set; }
}
