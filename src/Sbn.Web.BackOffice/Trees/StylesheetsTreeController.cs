using Sbn.Cms.Core;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.IO;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Trees;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Web.BackOffice.Trees
{
    [CoreTree]
    [Tree(Constants.Applications.Settings, Constants.Trees.Stylesheets, TreeTitle = "Stylesheets", SortOrder = 9, TreeGroup = Constants.Trees.Groups.Templating)]
    public class StylesheetsTreeController : FileSystemTreeController
    {
        protected override IFileSystem FileSystem { get; }

        private static readonly string[] ExtensionsStatic = { "css" };

        protected override string[] Extensions => ExtensionsStatic;

        protected override string FileIcon => "icon-brackets";

        public StylesheetsTreeController(
            ILocalizedTextService localizedTextService,
            SbnApiControllerTypeCollection sbnApiControllerTypeCollection,
            IMenuItemCollectionFactory menuItemCollectionFactory,
            FileSystems fileSystems,
            IEventAggregator eventAggregator)
            : base(localizedTextService, sbnApiControllerTypeCollection, menuItemCollectionFactory, eventAggregator)
        {
            FileSystem = fileSystems.StylesheetsFileSystem;
        }
    }
}
