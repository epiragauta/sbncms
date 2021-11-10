// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Web.BackOffice.Controllers;
using Sbn.Cms.Web.Common.Attributes;
using Sbn.Extensions;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Web.BackOffice.PropertyEditors
{
    [PluginController(Constants.Web.Mvc.BackOfficeApiArea)]
    public class NestedContentController : SbnAuthorizedJsonController
    {
        private readonly IContentTypeService _contentTypeService;

        public NestedContentController(IContentTypeService contentTypeService)
        {
            _contentTypeService = contentTypeService;
        }

        [HttpGet]
        public IEnumerable<object> GetContentTypes() => _contentTypeService
                .GetAllElementTypes()
                .OrderBy(x => x.SortOrder)
                .Select(x => new
                {
                    id = x.Id,
                    guid = x.Key,
                    name = x.Name,
                    alias = x.Alias,
                    icon = x.Icon,
                    tabs = x.CompositionPropertyGroups.Where(x => x.Type == PropertyGroupType.Group && x.GetParentAlias() is null).Select(y => y.Name).Distinct()
                });
    }
}
