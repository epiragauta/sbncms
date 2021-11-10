using Sbn.Cms.Core.Models.ContentEditing;
using Sbn.Cms.Core.Web;

namespace Sbn.Cms.Core.Notifications
{
    public class SendingMediaNotification : INotification
    {
        public ISbnContext SbnContext { get; }

        public MediaItemDisplay Media { get; }

        public SendingMediaNotification(MediaItemDisplay media, ISbnContext sbnContext)
        {
            Media = media;
            SbnContext = sbnContext;
        }
    }
}
