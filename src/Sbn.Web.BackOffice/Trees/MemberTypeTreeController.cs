using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Models.ContentEditing;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Trees;
using Sbn.Cms.Infrastructure.Search;
using Sbn.Cms.Web.Common.Attributes;
using Sbn.Cms.Web.Common.Authorization;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Web.BackOffice.Trees
{
    [CoreTree]
    [Authorize(Policy = AuthorizationPolicies.TreeAccessMemberTypes)]
    [Tree(Constants.Applications.Settings, Constants.Trees.MemberTypes, SortOrder = 2, TreeGroup = Constants.Trees.Groups.Settings)]
    [PluginController(Constants.Web.Mvc.BackOfficeTreeArea)]
    public class MemberTypeTreeController : MemberTypeAndGroupTreeControllerBase, ISearchableTree
    {
        private readonly SbnTreeSearcher _treeSearcher;
        private readonly IMemberTypeService _memberTypeService;

        public MemberTypeTreeController(
            ILocalizedTextService localizedTextService,
            SbnApiControllerTypeCollection sbnApiControllerTypeCollection,
            IMenuItemCollectionFactory menuItemCollectionFactory,
            SbnTreeSearcher treeSearcher,
            IMemberTypeService memberTypeService,
            IEventAggregator eventAggregator)
            : base(localizedTextService, sbnApiControllerTypeCollection, menuItemCollectionFactory, eventAggregator)
        {
            _treeSearcher = treeSearcher;
            _memberTypeService = memberTypeService;
        }


        protected override ActionResult<TreeNode> CreateRootNode(FormCollection queryStrings)
        {
            var rootResult = base.CreateRootNode(queryStrings);
            if (!(rootResult.Result is null))
            {
                return rootResult;
            }
            var root = rootResult.Value;

            //check if there are any member types
            root.HasChildren = _memberTypeService.GetAll().Any();
            return root;
        }
        protected override IEnumerable<TreeNode> GetTreeNodesFromService(string id, FormCollection queryStrings)
        {
            return _memberTypeService.GetAll()
                .OrderBy(x => x.Name)
                .Select(dt => CreateTreeNode(dt, Constants.ObjectTypes.MemberType, id, queryStrings, dt?.Icon ?? Constants.Icons.MemberType, false));
        }

        public IEnumerable<SearchResultEntity> Search(string query, int pageSize, long pageIndex, out long totalFound, string searchFrom = null)
            => _treeSearcher.EntitySearch(SbnObjectTypes.MemberType, query, pageSize, pageIndex, out totalFound, searchFrom);

    }
}
