using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Actions;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models.Trees;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Trees;
using Sbn.Cms.Web.Common.Attributes;
using Sbn.Cms.Web.Common.Authorization;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Web.BackOffice.Trees
{
    [Authorize(Policy = AuthorizationPolicies.TreeAccessMacros)]
    [Tree(Constants.Applications.Settings, Constants.Trees.Macros, TreeTitle = "Macros", SortOrder = 4, TreeGroup = Constants.Trees.Groups.Settings)]
    [PluginController(Constants.Web.Mvc.BackOfficeTreeArea)]
    [CoreTree]
    public class MacrosTreeController : TreeController
    {
        private readonly IMenuItemCollectionFactory _menuItemCollectionFactory;
        private readonly IMacroService  _macroService;

        public MacrosTreeController(ILocalizedTextService localizedTextService, SbnApiControllerTypeCollection sbnApiControllerTypeCollection, IMenuItemCollectionFactory menuItemCollectionFactory, IMacroService macroService, IEventAggregator eventAggregator) : base(localizedTextService, sbnApiControllerTypeCollection, eventAggregator)
        {
            _menuItemCollectionFactory = menuItemCollectionFactory;
            _macroService = macroService;
        }

        protected override ActionResult<TreeNode> CreateRootNode(FormCollection queryStrings)
        {
            var rootResult = base.CreateRootNode(queryStrings);
            if (!(rootResult.Result is null))
            {
                return rootResult;
            }
            var root = rootResult.Value;

            //check if there are any macros
            root.HasChildren = _macroService.GetAll().Any();
            return root;
        }

        protected override ActionResult<TreeNodeCollection> GetTreeNodes(string id, FormCollection queryStrings)
        {
            var nodes = new TreeNodeCollection();

            if (id == Constants.System.RootString)
            {
                foreach (var macro in _macroService.GetAll().OrderBy(m => m.Name))
                {
                    nodes.Add(CreateTreeNode(
                        macro.Id.ToString(),
                        id,
                        queryStrings,
                        macro.Name,
                        Constants.Icons.Macro,
                        false));
                }
            }

            return nodes;
        }

        protected override ActionResult<MenuItemCollection> GetMenuForNode(string id, FormCollection queryStrings)
        {
            var menu = _menuItemCollectionFactory.Create();

            if (id == Constants.System.RootString)
            {
                //Create the normal create action
                menu.Items.Add<ActionNew>(LocalizedTextService);

                //refresh action
                menu.Items.Add(new RefreshNode(LocalizedTextService, true));

                return menu;
            }

            var macro = _macroService.GetById(int.Parse(id, CultureInfo.InvariantCulture));
            if (macro == null) return menu;

            //add delete option for all macros
            menu.Items.Add<ActionDelete>(LocalizedTextService, opensDialog: true);

            return menu;
        }
    }
}
