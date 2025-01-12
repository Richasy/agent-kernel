// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Draw;

/// <summary>
/// Represents a client that can draw image.
/// </summary>
public interface IDrawClient : IDisposable
{
    /// <summary>
    /// Gets metadata that describes the <see cref="IDrawClient"/>.
    /// </summary>
    public DrawClientMetadata Metadata { get; }

    /// <summary>
    /// Draw an image.
    /// </summary>
    /// <param name="prompt">Image prompt.</param>
    /// <param name="options">Draw options.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns><see cref="BinaryData"/>.</returns>
    Task<BinaryData> DrawAsync(
        string prompt,
        DrawOptions? options,
        CancellationToken cancellationToken = default);
}
