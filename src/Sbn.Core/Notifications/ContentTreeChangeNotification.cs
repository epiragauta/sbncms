using System.Collections.Generic;
using System.Linq;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Services.Changes;

namespace Sbn.Cms.Core.Notifications
{
    public class ContentTreeChangeNotification : TreeChangeNotification<IContent>
    {
        public ContentTreeChangeNotification(TreeChange<IContent> target, EventMessages messages) : base(target, messages)
        {
        }

        public ContentTreeChangeNotification(IEnumerable<TreeChange<IContent>> target, EventMessages messages) : base(target, messages)
        {
        }

        public ContentTreeChangeNotification(IEnumerable<IContent> target,
            TreeChangeTypes changeTypes,
            EventMessages messages) : base(target.Select(x => new TreeChange<IContent>(x, changeTypes)), messages)
        {
        }

        public ContentTreeChangeNotification(IContent target,
            TreeChangeTypes changeTypes,
            EventMessages messages) : base(new TreeChange<IContent>(target, changeTypes), messages)
        {
        }
    }
}
