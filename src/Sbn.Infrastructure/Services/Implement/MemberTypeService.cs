using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Notifications;
using Sbn.Cms.Core.Persistence.Repositories;
using Sbn.Cms.Core.Scoping;
using Sbn.Cms.Core.Services.Changes;
using Sbn.Extensions;

namespace Sbn.Cms.Core.Services.Implement
{
    public class MemberTypeService : ContentTypeServiceBase<IMemberTypeRepository, IMemberType>, IMemberTypeService
    {
        private readonly IMemberTypeRepository _memberTypeRepository;

        public MemberTypeService(IScopeProvider provider, ILoggerFactory loggerFactory, IEventMessagesFactory eventMessagesFactory, IMemberService memberService,
            IMemberTypeRepository memberTypeRepository, IAuditRepository auditRepository, IEntityRepository entityRepository, IEventAggregator eventAggregator)
            : base(provider, loggerFactory, eventMessagesFactory, memberTypeRepository, auditRepository, null, entityRepository, eventAggregator)
        {
            MemberService = memberService;
            _memberTypeRepository = memberTypeRepository;
        }

        // beware! order is important to avoid deadlocks
        protected override int[] ReadLockIds { get; } = { Cms.Core.Constants.Locks.MemberTypes };
        protected override int[] WriteLockIds { get; } = { Cms.Core.Constants.Locks.MemberTree, Cms.Core.Constants.Locks.MemberTypes };

        private IMemberService MemberService { get; }

        protected override Guid ContainedObjectType => Cms.Core.Constants.ObjectTypes.MemberType;

        #region Notifications

        protected override SavingNotification<IMemberType> GetSavingNotification(IMemberType item,
            EventMessages eventMessages) => new MemberTypeSavingNotification(item, eventMessages);

        protected override SavingNotification<IMemberType> GetSavingNotification(IEnumerable<IMemberType> items,
            EventMessages eventMessages) => new MemberTypeSavingNotification(items, eventMessages);

        protected override SavedNotification<IMemberType> GetSavedNotification(IMemberType item,
            EventMessages eventMessages) => new MemberTypeSavedNotification(item, eventMessages);

        protected override SavedNotification<IMemberType> GetSavedNotification(IEnumerable<IMemberType> items,
            EventMessages eventMessages) => new MemberTypeSavedNotification(items, eventMessages);

        protected override DeletingNotification<IMemberType> GetDeletingNotification(IMemberType item,
            EventMessages eventMessages) => new MemberTypeDeletingNotification(item, eventMessages);

        protected override DeletingNotification<IMemberType> GetDeletingNotification(IEnumerable<IMemberType> items,
            EventMessages eventMessages) => new MemberTypeDeletingNotification(items, eventMessages);

        protected override DeletedNotification<IMemberType> GetDeletedNotification(IEnumerable<IMemberType> items,
            EventMessages eventMessages) => new MemberTypeDeletedNotification(items, eventMessages);

        protected override MovingNotification<IMemberType> GetMovingNotification(MoveEventInfo<IMemberType> moveInfo,
            EventMessages eventMessages) => new MemberTypeMovingNotification(moveInfo, eventMessages);

        protected override MovedNotification<IMemberType> GetMovedNotification(
            IEnumerable<MoveEventInfo<IMemberType>> moveInfo, EventMessages eventMessages) =>
            new MemberTypeMovedNotification(moveInfo, eventMessages);

        protected override ContentTypeChangeNotification<IMemberType> GetContentTypeChangedNotification(
            IEnumerable<ContentTypeChange<IMemberType>> changes, EventMessages eventMessages) =>
            new MemberTypeChangedNotification(changes, eventMessages);

        protected override ContentTypeRefreshNotification<IMemberType> GetContentTypeRefreshedNotification(
            IEnumerable<ContentTypeChange<IMemberType>> changes, EventMessages eventMessages) =>
            new MemberTypeRefreshedNotification(changes, eventMessages);

        #endregion

        protected override void DeleteItemsOfTypes(IEnumerable<int> typeIds)
        {
            foreach (var typeId in typeIds)
                MemberService.DeleteMembersOfType(typeId);
        }

        public string GetDefault()
        {
            using (var scope = ScopeProvider.CreateScope(autoComplete: true))
            {
                scope.ReadLock(ReadLockIds);

                using (var e = _memberTypeRepository.GetMany(new int[0]).GetEnumerator())
                {
                    if (e.MoveNext() == false)
                        throw new InvalidOperationException("No member types could be resolved");
                    var first = e.Current.Alias;
                    var current = true;
                    while (e.Current.Alias.InvariantEquals("Member") == false && (current = e.MoveNext()))
                    { }
                    return current ? e.Current.Alias : first;
                }
            }
        }
    }
}
