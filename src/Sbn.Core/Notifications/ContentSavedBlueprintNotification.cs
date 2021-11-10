// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Notifications
{
    public sealed class ContentSavedBlueprintNotification : ObjectNotification<IContent>
    {
        public ContentSavedBlueprintNotification(IContent target, EventMessages messages) : base(target, messages)
        {
        }

        public IContent SavedBlueprint => Target;
    }
}
