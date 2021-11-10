using System;
using Sbn.Cms.Core.Models.Email;

namespace Sbn.Cms.Core.Events
{
    public class SendEmailEventArgs : EventArgs
    {
        public EmailMessage Message { get; }

        public SendEmailEventArgs(EmailMessage message)
        {
            Message = message;
        }
    }
}
