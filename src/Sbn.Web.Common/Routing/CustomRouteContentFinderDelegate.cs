using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Sbn.Cms.Core.Models.PublishedContent;

namespace Sbn.Cms.Web.Common.Routing
{
    internal class CustomRouteContentFinderDelegate
    {
        private readonly Func<ActionExecutingContext, IPublishedContent> _findContent;

        public CustomRouteContentFinderDelegate(Func<ActionExecutingContext, IPublishedContent> findContent) => _findContent = findContent;

        public IPublishedContent FindContent(ActionExecutingContext actionExecutingContext) => _findContent(actionExecutingContext);
    }
}
