using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models.ContentEditing;
using Sbn.Cms.Core.Web;

namespace Sbn.Cms.Web.BackOffice.Filters
{

    /// <summary>
    /// Automatically checks if any request is a non-GET and if the
    /// resulting message is INotificationModel in which case it will append any Event Messages
    /// currently in the request.
    /// </summary>
    internal sealed class AppendCurrentEventMessagesAttribute : TypeFilterAttribute
    {
        public AppendCurrentEventMessagesAttribute() : base(typeof(AppendCurrentEventMessagesFilter))
        {
        }

        private class AppendCurrentEventMessagesFilter : IActionFilter
        {
            private readonly ISbnContextAccessor _sbnContextAccessor;
            private readonly IEventMessagesFactory _eventMessagesFactory;

            public AppendCurrentEventMessagesFilter(ISbnContextAccessor sbnContextAccessor, IEventMessagesFactory eventMessagesFactory)
            {
                _sbnContextAccessor = sbnContextAccessor;
                _eventMessagesFactory = eventMessagesFactory;
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
                if (context.HttpContext.Response == null) return;
                if (context.HttpContext.Request.Method.Equals(HttpMethod.Get.ToString(), StringComparison.InvariantCultureIgnoreCase)) return;
                if (!_sbnContextAccessor.TryGetSbnContext(out _))
                {
                    return;
                }

                if (!(context.Result is ObjectResult obj)) return;

                var notifications = obj.Value as INotificationModel;
                if (notifications == null) return;

                var msgs = _eventMessagesFactory.GetOrDefault();
                if (msgs == null) return;

                foreach (var eventMessage in msgs.GetAll())
                {
                    NotificationStyle msgType;
                    switch (eventMessage.MessageType)
                    {
                        case EventMessageType.Default:
                            msgType = NotificationStyle.Save;
                            break;
                        case EventMessageType.Info:
                            msgType = NotificationStyle.Info;
                            break;
                        case EventMessageType.Error:
                            msgType = NotificationStyle.Error;
                            break;
                        case EventMessageType.Success:
                            msgType = NotificationStyle.Success;
                            break;
                        case EventMessageType.Warning:
                            msgType = NotificationStyle.Warning;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    notifications.Notifications.Add(new BackOfficeNotification
                    {
                        Message = eventMessage.Message,
                        Header = eventMessage.Category,
                        NotificationType = msgType
                    });
                }
            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
            }
        }
    }
}
