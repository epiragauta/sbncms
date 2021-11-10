using System;
using Sbn.Cms.Web.Common.Controllers;

namespace Sbn.Cms.Web.Website.Controllers
{
    /// <summary>
    /// The defaults used for rendering Sbn front-end pages
    /// </summary>
    public class SbnRenderingDefaultsOptions
    {
        /// <summary>
        /// Gets the default sbn render controller type
        /// </summary>
        public Type DefaultControllerType { get; set; } = typeof(RenderController);
    }
}
