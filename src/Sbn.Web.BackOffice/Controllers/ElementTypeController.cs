// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Web.Common.Attributes;
using Sbn.Extensions;

namespace Sbn.Cms.Web.BackOffice.Controllers
{
    [PluginController("SbnApi")]
    public class ElementTypeController : SbnAuthorizedJsonController
    {
        private readonly IContentTypeService _contentTypeService;

        public ElementTypeController(IContentTypeService contentTypeService)
        {
            _contentTypeService = contentTypeService;
        }

        [HttpGet]
        public IEnumerable<object> GetAll()
        {
            return _contentTypeService
                .GetAllElementTypes()
                .OrderBy(x => x.SortOrder)
                .Select(x => new
                {
                    id = x.Id,
                    key = x.Key,
                    name = x.Name,
                    description = x.Description,
                    alias = x.Alias,
                    icon = x.Icon
                });
        }
    }
}
