// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Draw;

/// <summary>
/// Provides methods to draw.
/// </summary>
public interface IDrawService
{
    /// <summary>
    /// Gets the client that is used to draw.
    /// </summary>
    IDrawClient? Client { get; }

    /// <summary>
    /// Gets the configuration of the service.
    /// </summary>
    AIServiceConfig? Config { get; }

    /// <summary>
    /// Initialize the service.
    /// </summary>
    /// <param name="config">Configuration.</param>
    void Initialize(AIServiceConfig config);

    /// <summary>
    /// Get the predefined models.
    /// </summary>
    /// <returns>Model list.</returns>
    IReadOnlyList<DrawModel> GetPredefinedModels();
}
