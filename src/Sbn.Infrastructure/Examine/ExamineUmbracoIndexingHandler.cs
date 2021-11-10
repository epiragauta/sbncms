using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Examine;
using Examine.Search;
using Microsoft.Extensions.Logging;
using Sbn.Cms.Core.Logging;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Runtime;
using Sbn.Cms.Core.Scoping;
using Sbn.Cms.Infrastructure.HostedServices;
using Sbn.Cms.Infrastructure.Search;
using Sbn.Extensions;

namespace Sbn.Cms.Infrastructure.Examine
{
    /// <summary>
    /// Indexing handler for Examine indexes
    /// </summary>
    internal class ExamineSbnIndexingHandler : ISbnIndexingHandler
    {
        // the default enlist priority is 100
        // enlist with a lower priority to ensure that anything "default" runs after us
        // but greater that SafeXmlReaderWriter priority which is 60
        private const int EnlistPriority = 80;
        private readonly IMainDom _mainDom;
        private readonly ILogger<ExamineSbnIndexingHandler> _logger;
        private readonly IProfilingLogger _profilingLogger;
        private readonly IScopeProvider _scopeProvider;
        private readonly IExamineManager _examineManager;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        private readonly IContentValueSetBuilder _contentValueSetBuilder;
        private readonly IPublishedContentValueSetBuilder _publishedContentValueSetBuilder;
        private readonly IValueSetBuilder<IMedia> _mediaValueSetBuilder;
        private readonly IValueSetBuilder<IMember> _memberValueSetBuilder;
        private readonly Lazy<bool> _enabled;

        public ExamineSbnIndexingHandler(
            IMainDom mainDom,
            ILogger<ExamineSbnIndexingHandler> logger,
            IProfilingLogger profilingLogger,
            IScopeProvider scopeProvider,
            IExamineManager examineManager,
            IBackgroundTaskQueue backgroundTaskQueue,
            IContentValueSetBuilder contentValueSetBuilder,
            IPublishedContentValueSetBuilder publishedContentValueSetBuilder,
            IValueSetBuilder<IMedia> mediaValueSetBuilder,
            IValueSetBuilder<IMember> memberValueSetBuilder)
        {
            _mainDom = mainDom;
            _logger = logger;
            _profilingLogger = profilingLogger;
            _scopeProvider = scopeProvider;
            _examineManager = examineManager;
            _backgroundTaskQueue = backgroundTaskQueue;
            _contentValueSetBuilder = contentValueSetBuilder;
            _publishedContentValueSetBuilder = publishedContentValueSetBuilder;
            _mediaValueSetBuilder = mediaValueSetBuilder;
            _memberValueSetBuilder = memberValueSetBuilder;
            _enabled = new Lazy<bool>(IsEnabled);
        }

        /// <summary>
        /// Used to lazily check if Examine Index handling is enabled
        /// </summary>
        /// <returns></returns>
        private bool IsEnabled()
        {
            //let's deal with shutting down Examine with MainDom
            var examineShutdownRegistered = _mainDom.Register(release: () =>
            {
                using (_profilingLogger.TraceDuration<ExamineSbnIndexingHandler>("Examine shutting down"))
                {
                    _examineManager.Dispose();
                }
            });

            if (!examineShutdownRegistered)
            {
                _logger.LogInformation("Examine shutdown not registered, this AppDomain is not the MainDom, Examine will be disabled");

                //if we could not register the shutdown examine ourselves, it means we are not maindom! in this case all of examine should be disabled!
                Suspendable.ExamineEvents.SuspendIndexers(_logger);
                return false; //exit, do not continue
            }

            _logger.LogDebug("Examine shutdown registered with MainDom");

            var registeredIndexers = _examineManager.Indexes.OfType<ISbnIndex>().Count(x => x.EnableDefaultEventHandler);

            _logger.LogInformation("Adding examine event handlers for {RegisteredIndexers} index providers.", registeredIndexers);

            // don't bind event handlers if we're not suppose to listen
            if (registeredIndexers == 0)
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc />
        public bool Enabled => _enabled.Value;

        /// <inheritdoc />
        public void DeleteIndexForEntity(int entityId, bool keepIfUnpublished)
        {
            var actions = DeferedActions.Get(_scopeProvider);
            if (actions != null)
            {
                actions.Add(new DeferedDeleteIndex(this, entityId, keepIfUnpublished));
            }
            else
            {
                DeferedDeleteIndex.Execute(this, entityId, keepIfUnpublished);
            }
        }

        /// <inheritdoc />
        public void DeleteIndexForEntities(IReadOnlyCollection<int> entityIds, bool keepIfUnpublished)
        {
            var actions = DeferedActions.Get(_scopeProvider);
            if (actions != null)
            {
                actions.Add(new DeferedDeleteIndex(this, entityIds, keepIfUnpublished));
            }
            else
            {
                DeferedDeleteIndex.Execute(this, entityIds, keepIfUnpublished);
            }
        }

        /// <inheritdoc />
        public void ReIndexForContent(IContent sender, bool isPublished)
        {
            var actions = DeferedActions.Get(_scopeProvider);
            if (actions != null)
            {
                actions.Add(new DeferedReIndexForContent(_backgroundTaskQueue, this, sender, isPublished));
            }
            else
            {
                DeferedReIndexForContent.Execute(_backgroundTaskQueue, this, sender, isPublished);
            }
        }

        /// <inheritdoc />
        public void ReIndexForMedia(IMedia sender, bool isPublished)
        {
            var actions = DeferedActions.Get(_scopeProvider);
            if (actions != null)
            {
                actions.Add(new DeferedReIndexForMedia(_backgroundTaskQueue, this, sender, isPublished));
            }
            else
            {
                DeferedReIndexForMedia.Execute(_backgroundTaskQueue, this, sender, isPublished);
            }
        }

        /// <inheritdoc />
        public void ReIndexForMember(IMember member)
        {
            var actions = DeferedActions.Get(_scopeProvider);
            if (actions != null)
            {
                actions.Add(new DeferedReIndexForMember(_backgroundTaskQueue, this, member));
            }
            else
            {
                DeferedReIndexForMember.Execute(_backgroundTaskQueue, this, member);
            }
        }

        /// <inheritdoc />
        public void DeleteDocumentsForContentTypes(IReadOnlyCollection<int> removedContentTypes)
        {
            const int pageSize = 500;

            //Delete all content of this content/media/member type that is in any content indexer by looking up matched examine docs
            foreach (var id in removedContentTypes)
            {
                foreach (var index in _examineManager.Indexes.OfType<ISbnIndex>())
                {
                    var page = 0;
                    var total = long.MaxValue;
                    while (page * pageSize < total)
                    {
                        //paging with examine, see https://shazwazza.com/post/paging-with-examine/
                        var results = index.Searcher
                            .CreateQuery()
                            .Field("nodeType", id.ToInvariantString())
                            .Execute(QueryOptions.SkipTake(page * pageSize, pageSize));
                        total = results.TotalItemCount;
                        var paged = results.Skip(page * pageSize);

                        foreach (ISearchResult item in paged)
                        {
                            if (int.TryParse(item.Id, NumberStyles.Integer, CultureInfo.InvariantCulture, out int contentId))
                            {
                                DeleteIndexForEntity(contentId, false);
                            }
                        }

                        page++;
                    }
                }
            }
        }

        #region Deferred Actions
        private class DeferedActions
        {
            private readonly List<DeferedAction> _actions = new List<DeferedAction>();

            public static DeferedActions Get(IScopeProvider scopeProvider)
            {
                IScopeContext scopeContext = scopeProvider.Context;

                return scopeContext?.Enlist("examineEvents",
                    () => new DeferedActions(), // creator
                    (completed, actions) => // action
                    {
                        if (completed)
                        {
                            actions.Execute();
                        }
                    }, EnlistPriority);
            }

            public void Add(DeferedAction action) => _actions.Add(action);

            private void Execute()
            {
                foreach (DeferedAction action in _actions)
                {
                    action.Execute();
                }
            }
        }

        /// <summary>
        /// An action that will execute at the end of the Scope being completed
        /// </summary>
        private abstract class DeferedAction
        {
            public virtual void Execute()
            { }
        }

        /// <summary>
        /// Re-indexes an <see cref="IContent"/> item on a background thread
        /// </summary>
        private class DeferedReIndexForContent : DeferedAction
        {
            private readonly IBackgroundTaskQueue _backgroundTaskQueue;
            private readonly ExamineSbnIndexingHandler _examineSbnIndexingHandler;
            private readonly IContent _content;
            private readonly bool _isPublished;

            public DeferedReIndexForContent(IBackgroundTaskQueue backgroundTaskQueue, ExamineSbnIndexingHandler examineSbnIndexingHandler, IContent content, bool isPublished)
            {
                _backgroundTaskQueue = backgroundTaskQueue;
                _examineSbnIndexingHandler = examineSbnIndexingHandler;
                _content = content;
                _isPublished = isPublished;
            }

            public override void Execute() => Execute(_backgroundTaskQueue, _examineSbnIndexingHandler, _content, _isPublished);

            public static void Execute(IBackgroundTaskQueue backgroundTaskQueue, ExamineSbnIndexingHandler examineSbnIndexingHandler, IContent content, bool isPublished)
                => backgroundTaskQueue.QueueBackgroundWorkItem(cancellationToken =>
                {
                    using IScope scope = examineSbnIndexingHandler._scopeProvider.CreateScope(autoComplete: true);

                    // for content we have a different builder for published vs unpublished
                    // we don't want to build more value sets than is needed so we'll lazily build 2 one for published one for non-published
                    var builders = new Dictionary<bool, Lazy<List<ValueSet>>>
                    {
                        [true] = new Lazy<List<ValueSet>>(() => examineSbnIndexingHandler._publishedContentValueSetBuilder.GetValueSets(content).ToList()),
                        [false] = new Lazy<List<ValueSet>>(() => examineSbnIndexingHandler._contentValueSetBuilder.GetValueSets(content).ToList())
                    };

                    // This is only for content - so only index items for ISbnContentIndex (to exlude members)
                    foreach (ISbnIndex index in examineSbnIndexingHandler._examineManager.Indexes.OfType<ISbnContentIndex>()
                        //filter the indexers
                        .Where(x => isPublished || !x.PublishedValuesOnly)
                        .Where(x => x.EnableDefaultEventHandler))
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            return Task.CompletedTask;
                        }

                        List<ValueSet> valueSet = builders[index.PublishedValuesOnly].Value;
                        index.IndexItems(valueSet);
                    }

                    return Task.CompletedTask;
                });
        }

        /// <summary>
        /// Re-indexes an <see cref="IMedia"/> item on a background thread
        /// </summary>
        private class DeferedReIndexForMedia : DeferedAction
        {
            private readonly IBackgroundTaskQueue _backgroundTaskQueue;
            private readonly ExamineSbnIndexingHandler _examineSbnIndexingHandler;
            private readonly IMedia _media;
            private readonly bool _isPublished;

            public DeferedReIndexForMedia(IBackgroundTaskQueue backgroundTaskQueue, ExamineSbnIndexingHandler examineSbnIndexingHandler, IMedia media, bool isPublished)
            {
                _backgroundTaskQueue = backgroundTaskQueue;
                _examineSbnIndexingHandler = examineSbnIndexingHandler;
                _media = media;
                _isPublished = isPublished;
            }

            public override void Execute() => Execute(_backgroundTaskQueue, _examineSbnIndexingHandler, _media, _isPublished);

            public static void Execute(IBackgroundTaskQueue backgroundTaskQueue, ExamineSbnIndexingHandler examineSbnIndexingHandler, IMedia media, bool isPublished) =>
                // perform the ValueSet lookup on a background thread
                backgroundTaskQueue.QueueBackgroundWorkItem(cancellationToken =>
                {
                    using IScope scope = examineSbnIndexingHandler._scopeProvider.CreateScope(autoComplete: true);

                    var valueSet = examineSbnIndexingHandler._mediaValueSetBuilder.GetValueSets(media).ToList();

                    // This is only for content - so only index items for ISbnContentIndex (to exlude members)
                    foreach (ISbnIndex index in examineSbnIndexingHandler._examineManager.Indexes.OfType<ISbnContentIndex>()
                        //filter the indexers
                        .Where(x => isPublished || !x.PublishedValuesOnly)
                        .Where(x => x.EnableDefaultEventHandler))
                    {
                        index.IndexItems(valueSet);
                    }

                    return Task.CompletedTask;
                });
        }

        /// <summary>
        /// Re-indexes an <see cref="IMember"/> item on a background thread
        /// </summary>
        private class DeferedReIndexForMember : DeferedAction
        {
            private readonly ExamineSbnIndexingHandler _examineSbnIndexingHandler;
            private readonly IMember _member;
            private readonly IBackgroundTaskQueue _backgroundTaskQueue;

            public DeferedReIndexForMember(IBackgroundTaskQueue backgroundTaskQueue, ExamineSbnIndexingHandler examineSbnIndexingHandler, IMember member)
            {
                _examineSbnIndexingHandler = examineSbnIndexingHandler;
                _member = member;
                _backgroundTaskQueue = backgroundTaskQueue;
            }

            public override void Execute() => Execute(_backgroundTaskQueue, _examineSbnIndexingHandler, _member);

            public static void Execute(IBackgroundTaskQueue backgroundTaskQueue, ExamineSbnIndexingHandler examineSbnIndexingHandler, IMember member) =>
                // perform the ValueSet lookup on a background thread
                backgroundTaskQueue.QueueBackgroundWorkItem(cancellationToken =>
                {
                    using IScope scope = examineSbnIndexingHandler._scopeProvider.CreateScope(autoComplete: true);

                    var valueSet = examineSbnIndexingHandler._memberValueSetBuilder.GetValueSets(member).ToList();

                    // only process for ISbnMemberIndex (not content indexes)
                    foreach (ISbnIndex index in examineSbnIndexingHandler._examineManager.Indexes.OfType<ISbnMemberIndex>()
                        //filter the indexers
                        .Where(x => x.EnableDefaultEventHandler))
                    {
                        index.IndexItems(valueSet);
                    }

                    return Task.CompletedTask;
                });
        }

        private class DeferedDeleteIndex : DeferedAction
        {
            private readonly ExamineSbnIndexingHandler _examineSbnIndexingHandler;
            private readonly int _id;
            private readonly IReadOnlyCollection<int> _ids;
            private readonly bool _keepIfUnpublished;

            public DeferedDeleteIndex(ExamineSbnIndexingHandler examineSbnIndexingHandler, int id, bool keepIfUnpublished)
            {
                _examineSbnIndexingHandler = examineSbnIndexingHandler;
                _id = id;
                _keepIfUnpublished = keepIfUnpublished;
            }

            public DeferedDeleteIndex(ExamineSbnIndexingHandler examineSbnIndexingHandler, IReadOnlyCollection<int> ids, bool keepIfUnpublished)
            {
                _examineSbnIndexingHandler = examineSbnIndexingHandler;
                _ids = ids;
                _keepIfUnpublished = keepIfUnpublished;
            }

            public override void Execute()
            {
                if (_ids is null)
                {
                    Execute(_examineSbnIndexingHandler, _id, _keepIfUnpublished);
                }
                else
                {
                    Execute(_examineSbnIndexingHandler, _ids, _keepIfUnpublished);
                }
            }

            public static void Execute(ExamineSbnIndexingHandler examineSbnIndexingHandler, int id, bool keepIfUnpublished)
            {
                foreach (var index in examineSbnIndexingHandler._examineManager.Indexes.OfType<ISbnIndex>()
                    .Where(x => x.PublishedValuesOnly || !keepIfUnpublished)
                    .Where(x => x.EnableDefaultEventHandler))
                {
                    index.DeleteFromIndex(id.ToString(CultureInfo.InvariantCulture));
                }
            }

            public static void Execute(ExamineSbnIndexingHandler examineSbnIndexingHandler, IReadOnlyCollection<int> ids, bool keepIfUnpublished)
            {
                foreach (var index in examineSbnIndexingHandler._examineManager.Indexes.OfType<ISbnIndex>()
                    .Where(x => x.PublishedValuesOnly || !keepIfUnpublished)
                    .Where(x => x.EnableDefaultEventHandler))
                {
                    index.DeleteFromIndex(ids.Select(x => x.ToString(CultureInfo.InvariantCulture)));
                }
            }
        }
        #endregion
    }
}
