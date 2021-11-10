using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Mapping;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Models.ContentEditing;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Strings;
using Sbn.Cms.Web.Common.ActionsResults;
using Sbn.Cms.Web.Common.Attributes;
using Sbn.Cms.Web.Common.Authorization;
using Sbn.Extensions;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Web.BackOffice.Controllers
{
    /// <summary>
    /// The API controller for editing relation types.
    /// </summary>
    [PluginController(Constants.Web.Mvc.BackOfficeApiArea)]
    [Authorize(Policy = AuthorizationPolicies.TreeAccessRelationTypes)]
    [ParameterSwapControllerActionSelector(nameof(GetById), "id", typeof(int), typeof(Guid), typeof(Udi))]
    public class RelationTypeController : BackOfficeNotificationsController
    {
        private readonly ILogger<RelationTypeController> _logger;
        private readonly ISbnMapper _sbnMapper;
        private readonly IRelationService _relationService;
        private readonly IShortStringHelper _shortStringHelper;

        public RelationTypeController(
            ILogger<RelationTypeController> logger,
            ISbnMapper sbnMapper,
            IRelationService relationService,
            IShortStringHelper shortStringHelper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _sbnMapper = sbnMapper ?? throw new ArgumentNullException(nameof(sbnMapper));
            _relationService = relationService ?? throw new ArgumentNullException(nameof(relationService));
            _shortStringHelper = shortStringHelper ?? throw new ArgumentNullException(nameof(shortStringHelper));
        }

        /// <summary>
        /// Gets a relation type by id
        /// </summary>
        /// <param name="id">The relation type ID.</param>
        /// <returns>Returns the <see cref="RelationTypeDisplay"/>.</returns>
        public ActionResult<RelationTypeDisplay> GetById(int id)
        {
            var relationType = _relationService.GetRelationTypeById(id);

            if (relationType == null)
            {
                return NotFound();
            }

            var display = _sbnMapper.Map<IRelationType, RelationTypeDisplay>(relationType);

            return display;
        }
        /// <summary>
        /// Gets a relation type by guid
        /// </summary>
        /// <param name="id">The relation type ID.</param>
        /// <returns>Returns the <see cref="RelationTypeDisplay"/>.</returns>
        public ActionResult<RelationTypeDisplay> GetById(Guid id)
        {
            var relationType = _relationService.GetRelationTypeById(id);
            if (relationType == null)
            {
                return NotFound();
            }
            return _sbnMapper.Map<IRelationType, RelationTypeDisplay>(relationType);
        }

        /// <summary>
        /// Gets a relation type by udi
        /// </summary>
        /// <param name="id">The relation type ID.</param>
        /// <returns>Returns the <see cref="RelationTypeDisplay"/>.</returns>
        public ActionResult<RelationTypeDisplay> GetById(Udi id)
        {
            var guidUdi = id as GuidUdi;
            if (guidUdi == null)
                return NotFound();

            var relationType = _relationService.GetRelationTypeById(guidUdi.Guid);
            if (relationType == null)
            {
                return NotFound();
            }
            return _sbnMapper.Map<IRelationType, RelationTypeDisplay>(relationType);
        }

        public PagedResult<RelationDisplay> GetPagedResults(int id, int pageNumber = 1, int pageSize = 100)
        {

            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new NotSupportedException("Both pageNumber and pageSize must be greater than zero");
            }

            // Ordering do we need to pass through?
            var relations = _relationService.GetPagedByRelationTypeId(id, pageNumber -1, pageSize, out long totalRecords);

            return new PagedResult<RelationDisplay>(totalRecords, pageNumber, pageSize)
            {
                Items = relations.Select(x => _sbnMapper.Map<RelationDisplay>(x))
            };
        }

        /// <summary>
        /// Gets a list of object types which can be associated via relations.
        /// </summary>
        /// <returns>A list of available object types.</returns>
        public List<ObjectType> GetRelationObjectTypes()
        {
            var objectTypes = new List<ObjectType>
            {
                new ObjectType{Id = SbnObjectTypes.Document.GetGuid(), Name = SbnObjectTypes.Document.GetFriendlyName()},
                new ObjectType{Id = SbnObjectTypes.Media.GetGuid(), Name = SbnObjectTypes.Media.GetFriendlyName()},
                new ObjectType{Id = SbnObjectTypes.Member.GetGuid(), Name = SbnObjectTypes.Member.GetFriendlyName()},
                new ObjectType{Id = SbnObjectTypes.DocumentType.GetGuid(), Name = SbnObjectTypes.DocumentType.GetFriendlyName()},
                new ObjectType{Id = SbnObjectTypes.MediaType.GetGuid(), Name = SbnObjectTypes.MediaType.GetFriendlyName()},
                new ObjectType{Id = SbnObjectTypes.MemberType.GetGuid(), Name = SbnObjectTypes.MemberType.GetFriendlyName()},
                new ObjectType{Id = SbnObjectTypes.DataType.GetGuid(), Name = SbnObjectTypes.DataType.GetFriendlyName()},
                new ObjectType{Id = SbnObjectTypes.MemberGroup.GetGuid(), Name = SbnObjectTypes.MemberGroup.GetFriendlyName()},
                new ObjectType{Id = SbnObjectTypes.ROOT.GetGuid(), Name = SbnObjectTypes.ROOT.GetFriendlyName()},
                new ObjectType{Id = SbnObjectTypes.RecycleBin.GetGuid(), Name = SbnObjectTypes.RecycleBin.GetFriendlyName()},
            };

            return objectTypes;
        }

        /// <summary>
        /// Creates a new relation type.
        /// </summary>
        /// <param name="relationType">The relation type to create.</param>
        /// <returns>A <see cref="HttpResponseMessage"/> containing the persisted relation type's ID.</returns>
        public ActionResult<int> PostCreate(RelationTypeSave relationType)
        {
            var relationTypePersisted = new RelationType(
                relationType.Name,
                relationType.Name.ToSafeAlias(_shortStringHelper, true),
                relationType.IsBidirectional,
                relationType.ParentObjectType,
                relationType.ChildObjectType);

            try
            {
                _relationService.Save(relationTypePersisted);

                return relationTypePersisted.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating relation type with {Name}", relationType.Name);
                return ValidationProblem("Error creating relation type.");
            }
        }

        /// <summary>
        /// Updates an existing relation type.
        /// </summary>
        /// <param name="relationType">The relation type to update.</param>
        /// <returns>A display object containing the updated relation type.</returns>
        public ActionResult<RelationTypeDisplay> PostSave(RelationTypeSave relationType)
        {
            var relationTypePersisted = _relationService.GetRelationTypeById(relationType.Key);

            if (relationTypePersisted == null)
            {
                return ValidationProblem("Relation type does not exist");
            }

            _sbnMapper.Map(relationType, relationTypePersisted);

            try
            {
                _relationService.Save(relationTypePersisted);
                var display = _sbnMapper.Map<RelationTypeDisplay>(relationTypePersisted);
                display.AddSuccessNotification("Relation type saved", "");

                return display;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving relation type with {Id}", relationType.Id);
                return ValidationProblem("Something went wrong when saving the relation type");
            }
        }

        /// <summary>
        /// Deletes a relation type with a given ID.
        /// </summary>
        /// <param name="id">The ID of the relation type to delete.</param>
        /// <returns>A <see cref="HttpResponseMessage"/>.</returns>
        [HttpPost]
        [HttpDelete]
        public IActionResult DeleteById(int id)
        {
            var relationType = _relationService.GetRelationTypeById(id);

            if (relationType == null)
                return NotFound();

            _relationService.Delete(relationType);

            return Ok();
        }
    }
}
