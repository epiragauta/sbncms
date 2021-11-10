using Sbn.Cms.Core.Models.ContentEditing;
using Sbn.Cms.Core.Web;

namespace Sbn.Cms.Core.Notifications
{
    public class SendingContentNotification : INotification
    {
        public ISbnContext SbnContext { get; }

        public ContentItemDisplay Content { get; }

        public SendingContentNotification(ContentItemDisplay content, ISbnContext sbnContext)
        {
            Content = content;
            SbnContext = sbnContext;
        }
    }
}
