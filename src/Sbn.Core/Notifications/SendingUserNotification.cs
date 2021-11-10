using Sbn.Cms.Core.Models.ContentEditing;
using Sbn.Cms.Core.Web;

namespace Sbn.Cms.Core.Notifications
{
    public class SendingUserNotification : INotification
    {
        public ISbnContext SbnContext { get; }

        public UserDisplay User { get; }

        public SendingUserNotification(UserDisplay user, ISbnContext sbnContext)
        {
            User = user;
            SbnContext = sbnContext;
        }
    }
}
