using Sbn.Cms.Core;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.IO;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Trees;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Web.BackOffice.Trees
{
    [CoreTree]
    [Tree(Constants.Applications.Settings, Constants.Trees.Scripts, TreeTitle = "Scripts", SortOrder = 10, TreeGroup = Constants.Trees.Groups.Templating)]
    public class ScriptsTreeController : FileSystemTreeController
    {
        protected override IFileSystem FileSystem { get; }

        private static readonly string[] ExtensionsStatic = { "js" };

        protected override string[] Extensions => ExtensionsStatic;

        protected override string FileIcon => "icon-script";

        public ScriptsTreeController(
            ILocalizedTextService localizedTextService,
            SbnApiControllerTypeCollection sbnApiControllerTypeCollection,
            IMenuItemCollectionFactory menuItemCollectionFactory,
            FileSystems fileSystems,
            IEventAggregator eventAggregator)
            : base(localizedTextService, sbnApiControllerTypeCollection, menuItemCollectionFactory, eventAggregator)
        {
            FileSystem = fileSystems.ScriptsFileSystem;
        }
    }
}
