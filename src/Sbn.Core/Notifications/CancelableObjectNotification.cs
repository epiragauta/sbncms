// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.Events;

namespace Sbn.Cms.Core.Notifications
{
    public abstract class CancelableObjectNotification<T> : ObjectNotification<T>, ICancelableNotification where T : class
    {
        protected CancelableObjectNotification(T target, EventMessages messages) : base(target, messages)
        {
        }

        public bool Cancel { get; set; }

        public void CancelOperation(EventMessage cancelationMessage)
        {
            Cancel = true;
            cancelationMessage.IsDefaultEventMessage = true;
            Messages.Add(cancelationMessage);
        }
    }
}
