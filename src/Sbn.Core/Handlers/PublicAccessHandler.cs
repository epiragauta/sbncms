using System;
using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Notifications;
using Sbn.Cms.Core.Services;
using Sbn.Extensions;

namespace Sbn.Cms.Core.Handlers
{
    public sealed class PublicAccessHandler :
        INotificationHandler<MemberGroupSavedNotification>,
        INotificationHandler<MemberGroupDeletedNotification>
    {
        private readonly IPublicAccessService _publicAccessService;

        public PublicAccessHandler(IPublicAccessService publicAccessService) =>
            _publicAccessService = publicAccessService ?? throw new ArgumentNullException(nameof(publicAccessService));

        public void Handle(MemberGroupSavedNotification notification) => Handle(notification.SavedEntities);

        public void Handle(MemberGroupDeletedNotification notification) => Handle(notification.DeletedEntities);

        private void Handle(IEnumerable<IMemberGroup> affectedEntities)
        {
            foreach (var grp in affectedEntities)
            {
                //check if the name has changed
                if (grp.AdditionalData.ContainsKey("previousName")
                    && grp.AdditionalData["previousName"] != null
                    && grp.AdditionalData["previousName"].ToString().IsNullOrWhiteSpace() == false
                    && grp.AdditionalData["previousName"].ToString() != grp.Name)
                {
                    _publicAccessService.RenameMemberGroupRoleRules(grp.AdditionalData["previousName"].ToString(), grp.Name);
                }
            }
        }
    }
}
