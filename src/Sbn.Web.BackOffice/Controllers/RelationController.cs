using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Sbn.Cms.Core.Mapping;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Models.ContentEditing;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Web.Common.Attributes;
using Sbn.Cms.Web.Common.Authorization;
using Sbn.Extensions;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Web.BackOffice.Controllers
{
    [PluginController(Constants.Web.Mvc.BackOfficeApiArea)]
    [Authorize(Policy = AuthorizationPolicies.SectionAccessContent)]
    public class RelationController : SbnAuthorizedJsonController
    {
        private readonly ISbnMapper _sbnMapper;
        private readonly IRelationService _relationService;

        public RelationController(ISbnMapper sbnMapper,
            IRelationService relationService)
        {
            _sbnMapper = sbnMapper ?? throw new ArgumentNullException(nameof(sbnMapper));
            _relationService = relationService ?? throw new ArgumentNullException(nameof(relationService));
        }

        public RelationDisplay GetById(int id)
        {
            return _sbnMapper.Map<IRelation, RelationDisplay>(_relationService.GetById(id));
        }

        //[EnsureUserPermissionForContent("childId")]
        public IEnumerable<RelationDisplay> GetByChildId(int childId, string relationTypeAlias = "")
        {
            var relations = _relationService.GetByChildId(childId).ToArray();

            if (relations.Any() == false)
            {
                return Enumerable.Empty<RelationDisplay>();
            }

            if (string.IsNullOrWhiteSpace(relationTypeAlias) == false)
            {
                return
                    _sbnMapper.MapEnumerable<IRelation, RelationDisplay>(
                        relations.Where(x => x.RelationType.Alias.InvariantEquals(relationTypeAlias)));
            }

            return _sbnMapper.MapEnumerable<IRelation, RelationDisplay>(relations);
        }

    }
}
