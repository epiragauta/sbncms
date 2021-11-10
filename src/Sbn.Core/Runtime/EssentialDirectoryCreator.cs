using Microsoft.Extensions.Options;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.IO;
using Sbn.Cms.Core.Notifications;

namespace Sbn.Cms.Core.Runtime
{
    public class EssentialDirectoryCreator : INotificationHandler<SbnApplicationStartingNotification>
    {
        private readonly IIOHelper _ioHelper;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly GlobalSettings _globalSettings;

        public EssentialDirectoryCreator(IIOHelper ioHelper, IHostingEnvironment hostingEnvironment, IOptions<GlobalSettings> globalSettings)
        {
            _ioHelper = ioHelper;
            _hostingEnvironment = hostingEnvironment;
            _globalSettings = globalSettings.Value;
        }

        public void Handle(SbnApplicationStartingNotification notification)
        {
            // ensure we have some essential directories
            // every other component can then initialize safely
            _ioHelper.EnsurePathExists(_hostingEnvironment.MapPathContentRoot(Constants.SystemDirectories.Data));
            _ioHelper.EnsurePathExists(_hostingEnvironment.MapPathWebRoot(_globalSettings.SbnMediaPath));
            _ioHelper.EnsurePathExists(_hostingEnvironment.MapPathContentRoot(Constants.SystemDirectories.MvcViews));
            _ioHelper.EnsurePathExists(_hostingEnvironment.MapPathContentRoot(Constants.SystemDirectories.PartialViews));
            _ioHelper.EnsurePathExists(_hostingEnvironment.MapPathContentRoot(Constants.SystemDirectories.MacroPartials));

        }
    }
}
