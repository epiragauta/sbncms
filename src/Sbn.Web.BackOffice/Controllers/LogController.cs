using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.IO;
using Sbn.Cms.Core.Mapping;
using Sbn.Cms.Core.Media;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Models.ContentEditing;
using Sbn.Cms.Core.Security;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Infrastructure.Persistence;
using Sbn.Cms.Web.Common.Attributes;
using Sbn.Cms.Web.Common.Authorization;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Web.BackOffice.Controllers
{
    /// <summary>
    /// The API controller used for getting log history
    /// </summary>
    [PluginController(Constants.Web.Mvc.BackOfficeApiArea)]
    public class LogController : SbnAuthorizedJsonController
    {
        private readonly MediaFileManager _mediaFileManager;
        private readonly IImageUrlGenerator _imageUrlGenerator;
        private readonly IAuditService _auditService;
        private readonly ISbnMapper _sbnMapper;
        private readonly IBackOfficeSecurityAccessor _backofficeSecurityAccessor;
        private readonly IUserService _userService;
        private readonly AppCaches _appCaches;
        private readonly ISqlContext _sqlContext;

        public LogController(
            MediaFileManager mediaFileSystem,
            IImageUrlGenerator imageUrlGenerator,
            IAuditService auditService,
            ISbnMapper sbnMapper,
            IBackOfficeSecurityAccessor backofficeSecurityAccessor,
            IUserService userService,
            AppCaches appCaches,
            ISqlContext sqlContext)
         {
            _mediaFileManager = mediaFileSystem ?? throw new ArgumentNullException(nameof(mediaFileSystem));
            _imageUrlGenerator = imageUrlGenerator ?? throw new ArgumentNullException(nameof(imageUrlGenerator));
            _auditService = auditService ?? throw new ArgumentNullException(nameof(auditService));
            _sbnMapper = sbnMapper ?? throw new ArgumentNullException(nameof(sbnMapper));
            _backofficeSecurityAccessor = backofficeSecurityAccessor ?? throw new ArgumentNullException(nameof(backofficeSecurityAccessor));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _appCaches = appCaches ?? throw new ArgumentNullException(nameof(appCaches));
            _sqlContext = sqlContext ?? throw new ArgumentNullException(nameof(sqlContext));
         }

        [Authorize(Policy = AuthorizationPolicies.SectionAccessContentOrMedia)]
        public PagedResult<AuditLog> GetPagedEntityLog(int id,
            int pageNumber = 1,
            int pageSize = 10,
            Direction orderDirection = Direction.Descending,
            DateTime? sinceDate = null)
        {
            if (pageSize <= 0 || pageNumber <= 0)
            {
                return new PagedResult<AuditLog>(0, pageNumber, pageSize);
            }

            long totalRecords;
            var dateQuery = sinceDate.HasValue ? _sqlContext.Query<IAuditItem>().Where(x => x.CreateDate >= sinceDate) : null;
            var result = _auditService.GetPagedItemsByEntity(id, pageNumber - 1, pageSize, out totalRecords, orderDirection, customFilter: dateQuery);
            var mapped = result.Select(item => _sbnMapper.Map<AuditLog>(item));

            var page = new PagedResult<AuditLog>(totalRecords, pageNumber, pageSize)
            {
                Items = MapAvatarsAndNames(mapped)
            };

            return page;
        }

        public PagedResult<AuditLog> GetPagedCurrentUserLog(
            int pageNumber = 1,
            int pageSize = 10,
            Direction orderDirection = Direction.Descending,
            DateTime? sinceDate = null)
        {
            if (pageSize <= 0 || pageNumber <= 0)
            {
                return new PagedResult<AuditLog>(0, pageNumber, pageSize);
            }

            long totalRecords;
            var dateQuery = sinceDate.HasValue ? _sqlContext.Query<IAuditItem>().Where(x => x.CreateDate >= sinceDate) : null;
            var userId = _backofficeSecurityAccessor.BackOfficeSecurity.GetUserId().ResultOr(0);
            var result = _auditService.GetPagedItemsByUser(userId, pageNumber - 1, pageSize, out totalRecords, orderDirection, customFilter:dateQuery);
            var mapped = _sbnMapper.MapEnumerable<IAuditItem, AuditLog>(result);
            return new PagedResult<AuditLog>(totalRecords, pageNumber, pageSize)
            {
                Items = MapAvatarsAndNames(mapped)
            };
        }

        public IEnumerable<AuditLog> GetLog(AuditType logType, DateTime? sinceDate = null)
        {
            var result = _auditService.GetLogs(Enum<AuditType>.Parse(logType.ToString()), sinceDate);
            var mapped = _sbnMapper.MapEnumerable<IAuditItem, AuditLog>(result);
            return mapped;
        }

        private IEnumerable<AuditLog> MapAvatarsAndNames(IEnumerable<AuditLog> items)
        {
            var mappedItems = items.ToList();
            var userIds = mappedItems.Select(x => x.UserId).ToArray();
            var userAvatars = Enumerable.ToDictionary(_userService.GetUsersById(userIds), x => x.Id, x => x.GetUserAvatarUrls(_appCaches.RuntimeCache, _mediaFileManager, _imageUrlGenerator));
            var userNames = Enumerable.ToDictionary(_userService.GetUsersById(userIds), x => x.Id, x => x.Name);
            foreach (var item in mappedItems)
            {
                if (userAvatars.TryGetValue(item.UserId, out var avatars))
                {
                    item.UserAvatars = avatars;
                }
                if (userNames.TryGetValue(item.UserId, out var name))
                {
                    item.UserName = name;
                }


            }
            return mappedItems;
        }
    }
}
