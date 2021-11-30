// (c) gfoidl, all rights reserved

namespace gfoidl.Trx2Junit.Core.Abstractions;

/// <summary>
/// A helper / handler for file globbing.
/// </summary>
public interface IGlobHandler
{
    /// <summary>
    /// Expands wildcards from globbing, and updates <see cref="WorkerOptions.InputFiles"/>.
    /// </summary>
    /// <param name="options">The options to use for the glob expansion.</param>
    void ExpandWildcards(WorkerOptions options);
}
