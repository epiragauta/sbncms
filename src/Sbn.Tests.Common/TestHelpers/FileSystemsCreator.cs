using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.IO;

namespace Sbn.Cms.Tests.Common.TestHelpers
{
    public static class FileSystemsCreator
    {
        /// <summary>
        /// Create an instance FileSystems where you can set the individual filesystems.
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="ioHelper"></param>
        /// <param name="globalSettings"></param>
        /// <param name="hostingEnvironment"></param>
        /// <param name="macroPartialFileSystem"></param>
        /// <param name="partialViewsFileSystem"></param>
        /// <param name="stylesheetFileSystem"></param>
        /// <param name="scriptsFileSystem"></param>
        /// <param name="mvcViewFileSystem"></param>
        /// <returns></returns>
        public static FileSystems CreateTestFileSystems(
            ILoggerFactory loggerFactory,
            IIOHelper ioHelper,
            IOptions<GlobalSettings> globalSettings,
            IHostingEnvironment hostingEnvironment,
            IFileSystem macroPartialFileSystem,
            IFileSystem partialViewsFileSystem,
            IFileSystem stylesheetFileSystem,
            IFileSystem scriptsFileSystem,
            IFileSystem mvcViewFileSystem) =>
            new FileSystems(loggerFactory, ioHelper, globalSettings, hostingEnvironment, macroPartialFileSystem,
                partialViewsFileSystem, stylesheetFileSystem, scriptsFileSystem, mvcViewFileSystem);
    }
}
