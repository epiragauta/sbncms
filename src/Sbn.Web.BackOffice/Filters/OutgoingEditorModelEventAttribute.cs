using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sbn.Cms.Core.Dashboards;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Models.ContentEditing;
using Sbn.Cms.Core.Notifications;
using Sbn.Cms.Core.Security;
using Sbn.Cms.Core.Web;
using Sbn.Extensions;

namespace Sbn.Cms.Web.BackOffice.Filters
{
    /// <summary>
    /// Used to emit outgoing editor model events
    /// </summary>
    internal sealed class OutgoingEditorModelEventAttribute : TypeFilterAttribute
    {
        public OutgoingEditorModelEventAttribute() : base(typeof(OutgoingEditorModelEventFilter))
        {
        }


        private class OutgoingEditorModelEventFilter : IActionFilter
        {

            private readonly ISbnContextAccessor _sbnContextAccessor;

            private readonly IBackOfficeSecurityAccessor _backOfficeSecurityAccessor;

            private readonly IEventAggregator _eventAggregator;

            public OutgoingEditorModelEventFilter(
                ISbnContextAccessor sbnContextAccessor,
                IBackOfficeSecurityAccessor backOfficeSecurityAccessor, IEventAggregator eventAggregator)
            {
                _sbnContextAccessor = sbnContextAccessor
                                          ?? throw new ArgumentNullException(nameof(sbnContextAccessor));
                _backOfficeSecurityAccessor = backOfficeSecurityAccessor
                                              ?? throw new ArgumentNullException(nameof(backOfficeSecurityAccessor));
                _eventAggregator = eventAggregator
                                   ?? throw new ArgumentNullException(nameof(eventAggregator));
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
                if (context.Result == null) return;

                var sbnContext = _sbnContextAccessor.GetRequiredSbnContext();
                var currentUser = _backOfficeSecurityAccessor.BackOfficeSecurity.CurrentUser;
                if (currentUser == null) return;

                if (context.Result is ObjectResult objectContent)
                {
                    // Support both batch (dictionary) and single results
                    IEnumerable models;
                    if (objectContent.Value is IDictionary modelDictionary)
                    {
                        models = modelDictionary.Values;
                    }
                    else
                    {
                        models = new[] { objectContent.Value };
                    }

                    foreach (var model in models)
                    {
                        switch (model)
                        {
                            case ContentItemDisplay content:
                                _eventAggregator.Publish(new SendingContentNotification(content, sbnContext));
                                break;
                            case MediaItemDisplay media:
                                _eventAggregator.Publish(new SendingMediaNotification(media, sbnContext));
                                break;
                            case MemberDisplay member:
                                _eventAggregator.Publish(new SendingMemberNotification(member, sbnContext));
                                break;
                            case UserDisplay user:
                                _eventAggregator.Publish(new SendingUserNotification(user, sbnContext));
                                break;
                            case IEnumerable<Tab<IDashboardSlim>> dashboards:
                                _eventAggregator.Publish(new SendingDashboardsNotification(dashboards, sbnContext));
                                break;
                        }
                    }
                }
            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
            }
        }
    }
}
