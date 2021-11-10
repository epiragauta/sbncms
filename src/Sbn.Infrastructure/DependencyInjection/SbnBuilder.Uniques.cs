using System;
using Microsoft.Extensions.DependencyInjection;
using Sbn.Cms.Core.DependencyInjection;
using Sbn.Cms.Core.Dictionary;
using Sbn.Cms.Core.IO;
using Sbn.Cms.Core.Logging.Viewer;
using Sbn.Cms.Core.Models.PublishedContent;
using Sbn.Cms.Core.Strings;
using Sbn.Extensions;

namespace Sbn.Cms.Infrastructure.DependencyInjection
{
    /// <summary>
    /// Provides extension methods to the <see cref="ISbnBuilder"/> class.
    /// </summary>
    public static partial class SbnBuilderExtensions
    {
        /// <summary>
        /// Sets the culture dictionary factory.
        /// </summary>
        /// <typeparam name="T">The type of the factory.</typeparam>
        /// <param name="builder">The builder.</param>
        public static ISbnBuilder SetCultureDictionaryFactory<T>(this ISbnBuilder builder)
            where T : class, ICultureDictionaryFactory
        {
            builder.Services.AddUnique<ICultureDictionaryFactory, T>();
            return builder;
        }

        /// <summary>
        /// Sets the culture dictionary factory.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="factory">A function creating a culture dictionary factory.</param>
        public static ISbnBuilder SetCultureDictionaryFactory(this ISbnBuilder builder, Func<IServiceProvider, ICultureDictionaryFactory> factory)
        {
            builder.Services.AddUnique(factory);
            return builder;
        }

        /// <summary>
        /// Sets the culture dictionary factory.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="factory">A factory.</param>
        public static ISbnBuilder SetCultureDictionaryFactory(this ISbnBuilder builder, ICultureDictionaryFactory factory)
        {
            builder.Services.AddUnique(factory);
            return builder;
        }

        /// <summary>
        /// Sets the published content model factory.
        /// </summary>
        /// <typeparam name="T">The type of the factory.</typeparam>
        /// <param name="builder">The builder.</param>
        public static ISbnBuilder SetPublishedContentModelFactory<T>(this ISbnBuilder builder)
            where T : class, IPublishedModelFactory
        {
            builder.Services.AddUnique<IPublishedModelFactory, T>();
            return builder;
        }

        /// <summary>
        /// Sets the published content model factory.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="factory">A function creating a published content model factory.</param>
        public static ISbnBuilder SetPublishedContentModelFactory(this ISbnBuilder builder, Func<IServiceProvider, IPublishedModelFactory> factory)
        {
            builder.Services.AddUnique(factory);
            return builder;
        }

        /// <summary>
        /// Sets the published content model factory.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="factory">A published content model factory.</param>
        public static ISbnBuilder SetPublishedContentModelFactory(this ISbnBuilder builder, IPublishedModelFactory factory)
        {
            builder.Services.AddUnique(factory);
            return builder;
        }

        /// <summary>
        /// Sets the short string helper.
        /// </summary>
        /// <typeparam name="T">The type of the short string helper.</typeparam>
        /// <param name="builder">The builder.</param>
        public static ISbnBuilder SetShortStringHelper<T>(this ISbnBuilder builder)
            where T : class, IShortStringHelper
        {
            builder.Services.AddUnique<IShortStringHelper, T>();
            return builder;
        }

        /// <summary>
        /// Sets the short string helper.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="factory">A function creating a short string helper.</param>
        public static ISbnBuilder SetShortStringHelper(this ISbnBuilder builder, Func<IServiceProvider, IShortStringHelper> factory)
        {
            builder.Services.AddUnique(factory);
            return builder;
        }

        /// <summary>
        /// Sets the short string helper.
        /// </summary>
        /// <param name="builder">A builder.</param>
        /// <param name="helper">A short string helper.</param>
        public static ISbnBuilder SetShortStringHelper(this ISbnBuilder builder, IShortStringHelper helper)
        {
            builder.Services.AddUnique(helper);
            return builder;
        }

        /// <summary>
        /// Sets the filesystem used by the MediaFileManager
        /// </summary>
        /// <param name="builder">A builder.</param>
        /// <param name="filesystemFactory">Factory method to create an IFileSystem implementation used in the MediaFileManager</param>
        public static ISbnBuilder SetMediaFileSystem(this ISbnBuilder builder,
            Func<IServiceProvider, IFileSystem> filesystemFactory)
        {
            builder.Services.AddUnique(
                provider =>
                {
                    IFileSystem filesystem = filesystemFactory(provider);
                    // We need to use the Filesystems to create a shadow wrapper,
                    // because shadow wrapper requires the IsScoped delegate from the FileSystems.
                    // This is used by the scope provider when taking control of the filesystems.
                    FileSystems fileSystems = provider.GetRequiredService<FileSystems>();
                    IFileSystem shadow = fileSystems.CreateShadowWrapper(filesystem, "media");

                    return provider.CreateInstance<MediaFileManager>(shadow);
                });
            return builder;
        }

        /// <summary>
        /// Register FileSystems with a method to configure the <see cref="FileSystems"/>.
        /// </summary>
        /// <param name="builder">A builder.</param>
        /// <param name="configure">Method that configures the <see cref="FileSystems"/>.</param>
        /// <exception cref="ArgumentNullException">Throws exception if <paramref name="configure"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Throws exception if full path can't be resolved successfully.</exception>
        public static ISbnBuilder ConfigureFileSystems(this ISbnBuilder builder,
            Action<IServiceProvider, FileSystems> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.Services.AddUnique(
                provider =>
                {
                    FileSystems fileSystems = provider.CreateInstance<FileSystems>();
                    configure(provider, fileSystems);
                    return fileSystems;
                });
            return builder;
        }

        /// <summary>
        /// Sets the log viewer.
        /// </summary>
        /// <typeparam name="T">The type of the log viewer.</typeparam>
        /// <param name="builder">The builder.</param>
        public static ISbnBuilder SetLogViewer<T>(this ISbnBuilder builder)
            where T : class, ILogViewer
        {
            builder.Services.AddUnique<ILogViewer, T>();
            return builder;
        }

        /// <summary>
        /// Sets the log viewer.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="factory">A function creating a log viewer.</param>
        public static ISbnBuilder SetLogViewer(this ISbnBuilder builder, Func<IServiceProvider, ILogViewer> factory)
        {
            builder.Services.AddUnique(factory);
            return builder;
        }

        /// <summary>
        /// Sets the log viewer.
        /// </summary>
        /// <param name="builder">A builder.</param>
        /// <param name="viewer">A log viewer.</param>
        public static ISbnBuilder SetLogViewer(this ISbnBuilder builder, ILogViewer viewer)
        {
            builder.Services.AddUnique(viewer);
            return builder;
        }
    }
}
