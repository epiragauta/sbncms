using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Actions;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Models.ContentEditing;
using Sbn.Cms.Core.Models.Entities;
using Sbn.Cms.Core.Models.Trees;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Trees;
using Sbn.Cms.Infrastructure.Search;
using Sbn.Cms.Web.Common.Attributes;
using Sbn.Cms.Web.Common.Authorization;
using Sbn.Extensions;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Web.BackOffice.Trees
{
    [Authorize(Policy = AuthorizationPolicies.TreeAccessTemplates)]
    [Tree(Constants.Applications.Settings, Constants.Trees.Templates, SortOrder = 6, TreeGroup = Constants.Trees.Groups.Templating)]
    [PluginController(Constants.Web.Mvc.BackOfficeTreeArea)]
    [CoreTree]
    public class TemplatesTreeController : TreeController, ISearchableTree
    {
        private readonly SbnTreeSearcher _treeSearcher;
        private readonly IMenuItemCollectionFactory _menuItemCollectionFactory;
        private readonly IFileService _fileService;

        public TemplatesTreeController(
            SbnTreeSearcher treeSearcher,
            IMenuItemCollectionFactory menuItemCollectionFactory,
            ILocalizedTextService localizedTextService,
            SbnApiControllerTypeCollection sbnApiControllerTypeCollection,
            IFileService fileService,
            IEventAggregator eventAggregator
        ) : base(localizedTextService, sbnApiControllerTypeCollection, eventAggregator)
        {
            _treeSearcher = treeSearcher;
            _menuItemCollectionFactory = menuItemCollectionFactory;
            _fileService = fileService;
        }

        protected override ActionResult<TreeNode> CreateRootNode(FormCollection queryStrings)
        {
            var rootResult = base.CreateRootNode(queryStrings);
            if (!(rootResult.Result is null))
            {
                return rootResult;
            }
            var root = rootResult.Value;

            //check if there are any templates
            root.HasChildren = _fileService.GetTemplates(-1).Any();
            return root;
        }

        /// <summary>
        /// The method called to render the contents of the tree structure
        /// </summary>
        /// <param name="id"></param>
        /// <param name="queryStrings">
        /// All of the query string parameters passed from jsTree
        /// </param>
        /// <remarks>
        /// We are allowing an arbitrary number of query strings to be pased in so that developers are able to persist custom data from the front-end
        /// to the back end to be used in the query for model data.
        /// </remarks>
        protected override ActionResult<TreeNodeCollection> GetTreeNodes(string id, FormCollection queryStrings)
        {
            var nodes = new TreeNodeCollection();

            var found = id == Constants.System.RootString
                ? _fileService.GetTemplates(-1)
                : _fileService.GetTemplates(int.Parse(id, CultureInfo.InvariantCulture));

            nodes.AddRange(found.Select(template => CreateTreeNode(
                template.Id.ToString(CultureInfo.InvariantCulture),
                // TODO: Fix parent ID stuff for templates
                "-1",
                queryStrings,
                template.Name,
                template.IsMasterTemplate ? "icon-newspaper" : "icon-newspaper-alt",
                template.IsMasterTemplate,
                null,
                Udi.Create(ObjectTypes.GetUdiType(Constants.ObjectTypes.TemplateType), template.Key)
            )));

            return nodes;
        }

        /// <summary>
        /// Returns the menu structure for the node
        /// </summary>
        /// <param name="id"></param>
        /// <param name="queryStrings"></param>
        /// <returns></returns>
        protected override ActionResult<MenuItemCollection> GetMenuForNode(string id, FormCollection queryStrings)
        {
            var menu = _menuItemCollectionFactory.Create();

            //Create the normal create action
            var item = menu.Items.Add<ActionNew>(LocalizedTextService, opensDialog: true);
            item.NavigateToRoute($"{queryStrings.GetRequiredValue<string>("application")}/templates/edit/{id}?create=true");

            if (id == Constants.System.RootString)
            {
                //refresh action
                menu.Items.Add(new RefreshNode(LocalizedTextService, true));

                return menu;
            }

            var template = _fileService.GetTemplate(int.Parse(id, CultureInfo.InvariantCulture));
            if (template == null) return menu;
            var entity = FromTemplate(template);

            //don't allow delete if it has child layouts
            if (template.IsMasterTemplate == false)
            {
                //add delete option if it doesn't have children
                menu.Items.Add<ActionDelete>(LocalizedTextService, true, opensDialog: true);
            }

            //add refresh
            menu.Items.Add(new RefreshNode(LocalizedTextService, true));


            return menu;
        }

        private EntitySlim FromTemplate(ITemplate template)
        {
            return new EntitySlim
            {
                CreateDate = template.CreateDate,
                Id = template.Id,
                Key = template.Key,
                Name = template.Name,
                NodeObjectType = Constants.ObjectTypes.Template,
                // TODO: Fix parent/paths on templates
                ParentId = -1,
                Path = template.Path,
                UpdateDate = template.UpdateDate
            };
        }

        public IEnumerable<SearchResultEntity> Search(string query, int pageSize, long pageIndex, out long totalFound, string searchFrom = null)
            => _treeSearcher.EntitySearch(SbnObjectTypes.Template, query, pageSize, pageIndex, out totalFound, searchFrom);
    }
}
