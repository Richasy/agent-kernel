// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel.Models;

/// <summary>
/// Options for drawing.
/// </summary>
public class DrawOptions
{
    /// <summary>
    /// Gets or sets the image model.
    /// </summary>
    public string? ModelId { get; set; }

    /// <summary>
    /// Gets or sets the width of the image.
    /// </summary>
    public int? Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the image.
    /// </summary>
    public int? Height { get; set; }
}
