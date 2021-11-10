using System.Collections.Generic;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models.Membership;

namespace Sbn.Cms.Core.Notifications
{
    public class AssignedUserGroupPermissionsNotification : EnumerableObjectNotification<EntityPermission>
    {
        public AssignedUserGroupPermissionsNotification(IEnumerable<EntityPermission> target, EventMessages messages) : base(target, messages)
        {
        }

        public IEnumerable<EntityPermission> EntityPermissions => Target;
    }
}
