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
using Sbn.Extensions;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Web.BackOffice.Trees
{
    [Authorize(Policy = AuthorizationPolicies.TreeAccessRelationTypes)]
    [Tree(Constants.Applications.Settings, Constants.Trees.RelationTypes, SortOrder = 5, TreeGroup = Constants.Trees.Groups.Settings)]
    [PluginController(Constants.Web.Mvc.BackOfficeTreeArea)]
    [CoreTree]
    public class RelationTypeTreeController : TreeController
    {
        private readonly IMenuItemCollectionFactory _menuItemCollectionFactory;
        private readonly IRelationService _relationService;

        public RelationTypeTreeController(
            ILocalizedTextService localizedTextService,
            SbnApiControllerTypeCollection sbnApiControllerTypeCollection,
            IMenuItemCollectionFactory menuItemCollectionFactory,
            IRelationService relationService,
            IEventAggregator eventAggregator)
            : base(localizedTextService, sbnApiControllerTypeCollection, eventAggregator)
        {
            _menuItemCollectionFactory = menuItemCollectionFactory;
            _relationService = relationService;
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

            var relationType = _relationService.GetRelationTypeById(int.Parse(id, CultureInfo.InvariantCulture));
            if (relationType == null) return menu;

            if (relationType.IsSystemRelationType() == false)
            {
                menu.Items.Add<ActionDelete>(LocalizedTextService);
            }

            return menu;
        }

        protected override ActionResult<TreeNodeCollection> GetTreeNodes(string id, FormCollection queryStrings)
        {
            var nodes = new TreeNodeCollection();

            if (id == Constants.System.RootString)
            {
                nodes.AddRange(_relationService.GetAllRelationTypes()
                    .Select(rt => CreateTreeNode(rt.Id.ToString(), id, queryStrings, rt.Name,
                        "icon-trafic", false)));
            }

            return nodes;
        }
    }
}
