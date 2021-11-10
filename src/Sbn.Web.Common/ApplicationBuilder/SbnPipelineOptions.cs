using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;

namespace Sbn.Cms.Web.Common.ApplicationBuilder
{
    /// <summary>
    /// Options to allow modifying the <see cref="IApplicationBuilder"/> pipeline before and after Sbn registers it's core middlewares.
    /// </summary>
    public class SbnPipelineOptions
    {
        /// <summary>
        /// Returns a mutable list of all registered startup filters
        /// </summary>
        public IList<ISbnPipelineFilter> PipelineFilters { get; } = new List<ISbnPipelineFilter>();

        /// <summary>
        /// Adds a filter to the list
        /// </summary>
        /// <param name="filter"></param>
        public void AddFilter(ISbnPipelineFilter filter) => PipelineFilters.Add(filter);
    }
}
