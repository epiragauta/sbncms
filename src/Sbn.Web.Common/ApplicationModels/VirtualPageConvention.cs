using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Sbn.Cms.Web.Common.Filters;

namespace Sbn.Cms.Web.Common.ApplicationModels
{
    /// <summary>
    /// Adds the <see cref="SbnVirtualPageFilterAttribute"/> as a convention
    /// </summary>
    public class VirtualPageConvention : IActionModelConvention
    {
        /// <inheritdoc/>
        public void Apply(ActionModel action) => action.Filters.Add(new SbnVirtualPageFilterAttribute());
    }
}
