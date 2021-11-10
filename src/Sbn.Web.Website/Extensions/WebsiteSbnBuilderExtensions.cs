using System;
using Sbn.Cms.Core.DependencyInjection;
using Sbn.Cms.Core.Routing;

namespace Sbn.Extensions
{
    /// <summary>
    /// Provides extension methods to the <see cref="ISbnBuilder"/> class.
    /// </summary>
    public static class WebsiteSbnBuilderExtensions
    {
        #region Uniques

        /// <summary>
        /// Sets the content last chance finder.
        /// </summary>
        /// <typeparam name="T">The type of the content last chance finder.</typeparam>
        /// <param name="builder">The builder.</param>
        public static ISbnBuilder SetContentLastChanceFinder<T>(this ISbnBuilder builder)
            where T : class, IContentLastChanceFinder
        {
            builder.Services.AddUnique<IContentLastChanceFinder, T>();
            return builder;
        }

        /// <summary>
        /// Sets the content last chance finder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="factory">A function creating a last chance finder.</param>
        public static ISbnBuilder SetContentLastChanceFinder(this ISbnBuilder builder, Func<IServiceProvider, IContentLastChanceFinder> factory)
        {
            builder.Services.AddUnique(factory);
            return builder;
        }

        /// <summary>
        /// Sets the content last chance finder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="finder">A last chance finder.</param>
        public static ISbnBuilder SetContentLastChanceFinder(this ISbnBuilder builder, IContentLastChanceFinder finder)
        {
            builder.Services.AddUnique(finder);
            return builder;
        }

        /// <summary>
        /// Sets the site domain helper.
        /// </summary>
        /// <typeparam name="T">The type of the site domain helper.</typeparam>
        /// <param name="builder"></param>
        public static ISbnBuilder SetSiteDomainHelper<T>(this ISbnBuilder builder)
            where T : class, ISiteDomainMapper
        {
            builder.Services.AddUnique<ISiteDomainMapper, T>();
            return builder;
        }

        /// <summary>
        /// Sets the site domain helper.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="factory">A function creating a helper.</param>
        public static ISbnBuilder SetSiteDomainHelper(this ISbnBuilder builder, Func<IServiceProvider, ISiteDomainMapper> factory)
        {
            builder.Services.AddUnique(factory);
            return builder;
        }

        /// <summary>
        /// Sets the site domain helper.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="helper">A helper.</param>
        public static ISbnBuilder SetSiteDomainHelper(this ISbnBuilder builder, ISiteDomainMapper helper)
        {
            builder.Services.AddUnique(helper);
            return builder;
        }

        #endregion
    }
}
