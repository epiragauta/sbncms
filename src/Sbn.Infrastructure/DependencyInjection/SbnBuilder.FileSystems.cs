using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.DependencyInjection;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.IO;
using Sbn.Cms.Core.IO.MediaPathSchemes;
using Sbn.Extensions;

namespace Sbn.Cms.Infrastructure.DependencyInjection
{
    public static partial class SbnBuilderExtensions
    {
        /*
         * HOW TO REPLACE THE MEDIA UNDERLYING FILESYSTEM
         * ----------------------------------------------
         *
         * Create an implementation of IFileSystem and register it as the underlying filesystem for
         * MediaFileSystem with the following extension on composition.
         *
         * builder.SetMediaFileSystem(factory => FactoryMethodToReturnYourImplementation())
         *
         * WHAT IS SHADOWING
         * -----------------
         *
         * Shadowing is the technology used for Deploy to implement some sort of
         * transaction-management on top of filesystems. The plumbing explained above,
         * compared to creating your own physical filesystem, ensures that your filesystem
         * would participate into such transactions.
         *
         */

        internal static ISbnBuilder AddFileSystems(this ISbnBuilder builder)
        {
            // register FileSystems, which manages all filesystems
            builder.Services.AddUnique<FileSystems>();

            // register the scheme for media paths
            builder.Services.AddUnique<IMediaPathScheme, UniqueMediaPathScheme>();

            builder.SetMediaFileSystem(factory =>
            {
                IIOHelper ioHelper = factory.GetRequiredService<IIOHelper>();
                IHostingEnvironment hostingEnvironment = factory.GetRequiredService<IHostingEnvironment>();
                ILogger<PhysicalFileSystem> logger = factory.GetRequiredService<ILogger<PhysicalFileSystem>>();
                GlobalSettings globalSettings = factory.GetRequiredService<IOptions<GlobalSettings>>().Value;

                var rootPath = hostingEnvironment.MapPathWebRoot(globalSettings.SbnMediaPath);
                var rootUrl = hostingEnvironment.ToAbsolute(globalSettings.SbnMediaPath);
                return new PhysicalFileSystem(ioHelper, hostingEnvironment, logger, rootPath, rootUrl);
            });

            return builder;
        }
    }
}
