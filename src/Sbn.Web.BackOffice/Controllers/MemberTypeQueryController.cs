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
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Web.BackOffice.Controllers
{
    /// <summary>
    /// An API controller used for dealing with member types
    /// </summary>
    [PluginController(Constants.Web.Mvc.BackOfficeApiArea)]
    [Authorize(Policy = AuthorizationPolicies.TreeAccessMembersOrMemberTypes)]
    public class MemberTypeQueryController : BackOfficeNotificationsController
    {
        private readonly IMemberTypeService _memberTypeService;
        private readonly ISbnMapper _sbnMapper;


        public MemberTypeQueryController(
            IMemberTypeService memberTypeService,
            ISbnMapper sbnMapper)
        {
            _memberTypeService = memberTypeService ?? throw new ArgumentNullException(nameof(memberTypeService));
            _sbnMapper = sbnMapper ?? throw new ArgumentNullException(nameof(sbnMapper));
        }

        /// <summary>
        /// Returns all member types
        /// </summary>
        public IEnumerable<ContentTypeBasic> GetAllTypes() =>
            _memberTypeService.GetAll()
                .Select(_sbnMapper.Map<IMemberType, ContentTypeBasic>);

    }
}
