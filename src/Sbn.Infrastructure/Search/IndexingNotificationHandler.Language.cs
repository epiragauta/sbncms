using System;
using System.Linq;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Notifications;
using Sbn.Cms.Infrastructure.Examine;

namespace Sbn.Cms.Infrastructure.Search
{
    public sealed class LanguageIndexingNotificationHandler : INotificationHandler<LanguageCacheRefresherNotification>
    {
        private readonly ISbnIndexingHandler _sbnIndexingHandler;
        private readonly IIndexRebuilder _indexRebuilder;

        public LanguageIndexingNotificationHandler(ISbnIndexingHandler sbnIndexingHandler, IIndexRebuilder indexRebuilder)
        {
            _sbnIndexingHandler = sbnIndexingHandler ?? throw new ArgumentNullException(nameof(sbnIndexingHandler));
            _indexRebuilder = indexRebuilder ?? throw new ArgumentNullException(nameof(indexRebuilder));
        }

        public void Handle(LanguageCacheRefresherNotification args)
        {
            if (!_sbnIndexingHandler.Enabled)
            {
                return;
            }

            if (!(args.MessageObject is LanguageCacheRefresher.JsonPayload[] payloads))
            {
                return;
            }

            if (payloads.Length == 0)
            {
                return;
            }

            var removedOrCultureChanged = payloads.Any(x =>
                x.ChangeType == LanguageCacheRefresher.JsonPayload.LanguageChangeType.ChangeCulture
                    || x.ChangeType == LanguageCacheRefresher.JsonPayload.LanguageChangeType.Remove);

            if (removedOrCultureChanged)
            {
                //if a lang is removed or it's culture has changed, we need to rebuild the indexes since
                //field names and values in the index have a string culture value.
                _indexRebuilder.RebuildIndexes(false);
            }
        }
    }
}
