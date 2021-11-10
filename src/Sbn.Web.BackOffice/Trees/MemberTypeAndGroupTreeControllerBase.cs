using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Actions;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models.Trees;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Trees;
using Sbn.Cms.Web.Common.Attributes;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Web.BackOffice.Trees
{
    [PluginController(Constants.Web.Mvc.BackOfficeTreeArea)]
    [CoreTree]
    public abstract class MemberTypeAndGroupTreeControllerBase : TreeController
    {
        public IMenuItemCollectionFactory MenuItemCollectionFactory { get; }

        protected MemberTypeAndGroupTreeControllerBase(
            ILocalizedTextService localizedTextService,
            SbnApiControllerTypeCollection sbnApiControllerTypeCollection,
            IMenuItemCollectionFactory menuItemCollectionFactory,
            IEventAggregator eventAggregator)
            : base(localizedTextService, sbnApiControllerTypeCollection, eventAggregator)
        {
            MenuItemCollectionFactory = menuItemCollectionFactory;
        }

        protected override ActionResult<TreeNodeCollection> GetTreeNodes(string id, FormCollection queryStrings)
        {
            var nodes = new TreeNodeCollection();
            nodes.AddRange(GetTreeNodesFromService(id, queryStrings));
            return nodes;
        }

        protected override ActionResult<MenuItemCollection> GetMenuForNode(string id, FormCollection queryStrings)
        {
            var menu = MenuItemCollectionFactory.Create();

            if (id == Constants.System.RootString)
            {
                // root actions
                menu.Items.Add(new CreateChildEntity(LocalizedTextService));
                menu.Items.Add(new RefreshNode(LocalizedTextService, true));
                return menu;
            }
            else
            {
                //delete member type/group
                menu.Items.Add<ActionDelete>(LocalizedTextService, opensDialog: true);
            }

            return menu;
        }

        protected abstract IEnumerable<TreeNode> GetTreeNodesFromService(string id, FormCollection queryStrings);
    }
}
