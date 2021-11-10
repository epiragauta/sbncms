using System;
using System.Collections.Generic;
using Sbn.Cms.Core.Composing;
using Sbn.Cms.Web.Common.Controllers;

namespace Sbn.Extensions
{
    public static class TypeLoaderExtensions
    {
        /// <summary>
        /// Gets all types implementing <see cref="SbnApiController"/>.
        /// </summary>
        public static IEnumerable<Type> GetSbnApiControllers(this TypeLoader typeLoader)
            => typeLoader.GetTypes<SbnApiController>();
    }
}
