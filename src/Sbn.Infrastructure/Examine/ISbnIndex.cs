using System.Collections.Generic;
using Examine;

namespace Sbn.Cms.Infrastructure.Examine
{
    /// <summary>
    /// A Marker interface for defining an Sbn indexer
    /// </summary>
    public interface ISbnIndex : IIndex, IIndexStats
    {
        /// <summary>
        /// When set to true Sbn will keep the index in sync with Sbn data automatically
        /// </summary>
        bool EnableDefaultEventHandler { get; }

        /// <summary>
        /// When set to true the index will only retain published values
        /// </summary>
        /// <remarks>
        /// Any non-published values will not be put or kept in the index:
        /// * Deleted, Trashed, non-published Content items
        /// * non-published Variants
        /// </remarks>
        bool PublishedValuesOnly { get; }
    }
}
