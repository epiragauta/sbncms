using System.IO;
using Examine;
using Sbn.Cms.Core.Hosting;

namespace Sbn.Cms.Infrastructure.Examine
{
    /// <summary>
    /// Sets the Examine <see cref="IApplicationRoot"/> to be ExamineIndexes sub directory of the Sbn TEMP folder
    /// </summary>
    public class SbnApplicationRoot : IApplicationRoot
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public SbnApplicationRoot(IHostingEnvironment hostingEnvironment)
            => _hostingEnvironment = hostingEnvironment;

        public DirectoryInfo ApplicationRoot
            => new DirectoryInfo(
                Path.Combine(
                    _hostingEnvironment.MapPathContentRoot(Core.Constants.SystemDirectories.TempData),
                    "ExamineIndexes"));
    }
}
