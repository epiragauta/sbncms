using System;
using System.Collections.Generic;
using Sbn.Cms.Core.Composing;
using Sbn.Cms.Web.Common.Controllers;
using Sbn.Cms.Web.Website.Controllers;

namespace Sbn.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="TypeLoader"/> class.
    /// </summary>
    // Migrated to .NET Core
    public static class TypeLoaderExtensions
    {
        /// <summary>
        /// Gets all types implementing <see cref="SurfaceController"/>.
        /// </summary>
        internal static IEnumerable<Type> GetSurfaceControllers(this TypeLoader typeLoader)
            => typeLoader.GetTypes<SurfaceController>();

        /// <summary>
        /// Gets all types implementing <see cref="SbnApiController"/>.
        /// </summary>
        internal static IEnumerable<Type> GetSbnApiControllers(this TypeLoader typeLoader)
            => typeLoader.GetTypes<SbnApiController>();
    }
}
