using Sbn.Cms.Core.Models.ContentEditing;
using Sbn.Cms.Core.Web;

namespace Sbn.Cms.Core.Notifications
{
    public class SendingMemberNotification : INotification
    {
        public ISbnContext SbnContext { get; }

        public MemberDisplay Member { get; }

        public SendingMemberNotification(MemberDisplay member, ISbnContext sbnContext)
        {
            Member = member;
            SbnContext = sbnContext;
        }
    }
}
