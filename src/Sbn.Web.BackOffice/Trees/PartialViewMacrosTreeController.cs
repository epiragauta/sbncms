using Microsoft.AspNetCore.Authorization;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.IO;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Trees;
using Sbn.Cms.Web.Common.Attributes;
using Sbn.Cms.Web.Common.Authorization;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Web.BackOffice.Trees
{
    /// <summary>
    /// Tree for displaying partial view macros in the developer app
    /// </summary>
    [Tree(Constants.Applications.Settings, Constants.Trees.PartialViewMacros, SortOrder = 8, TreeGroup = Constants.Trees.Groups.Templating)]
    [Authorize(Policy = AuthorizationPolicies.TreeAccessPartialViewMacros)]
    [PluginController(Constants.Web.Mvc.BackOfficeTreeArea)]
    [CoreTree]
    public class PartialViewMacrosTreeController : PartialViewsTreeController
    {
        protected override IFileSystem FileSystem { get; }

        private static readonly string[] ExtensionsStatic = {"cshtml"};

        protected override string[] Extensions => ExtensionsStatic;

        protected override string FileIcon => "icon-article";

        public PartialViewMacrosTreeController(
            ILocalizedTextService localizedTextService,
            SbnApiControllerTypeCollection sbnApiControllerTypeCollection,
            IMenuItemCollectionFactory menuItemCollectionFactory,
            FileSystems fileSystems,
            IEventAggregator eventAggregator)
            : base(localizedTextService, sbnApiControllerTypeCollection, menuItemCollectionFactory, fileSystems, eventAggregator)
        {
            FileSystem = fileSystems.MacroPartialsFileSystem;
        }
    }
}
