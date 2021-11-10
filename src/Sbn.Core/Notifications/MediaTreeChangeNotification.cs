using System.Collections.Generic;
using System.Linq;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Services.Changes;

namespace Sbn.Cms.Core.Notifications
{
    public class MediaTreeChangeNotification : TreeChangeNotification<IMedia>
    {
        public MediaTreeChangeNotification(TreeChange<IMedia> target, EventMessages messages) : base(target, messages)
        {
        }

        public MediaTreeChangeNotification(IEnumerable<TreeChange<IMedia>> target, EventMessages messages) : base(target, messages)
        {
        }

        public MediaTreeChangeNotification(IEnumerable<IMedia> target,
            TreeChangeTypes changeTypes,
            EventMessages messages) : base(target.Select(x => new TreeChange<IMedia>(x, changeTypes)), messages)
        {
        }

        public MediaTreeChangeNotification(IMedia target, TreeChangeTypes changeTypes, EventMessages messages) : base(
            new TreeChange<IMedia>(target, changeTypes), messages)
        {
        }
    }
}
