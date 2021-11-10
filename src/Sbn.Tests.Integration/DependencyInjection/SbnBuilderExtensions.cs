// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Examine;
using Examine.Lucene.Directories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.DependencyInjection;
using Sbn.Cms.Core.Logging;
using Sbn.Cms.Core.Runtime;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Services.Implement;
using Sbn.Cms.Core.Sync;
using Sbn.Cms.Core.WebAssets;
using Sbn.Cms.Infrastructure.Examine;
using Sbn.Cms.Infrastructure.HostedServices;
using Sbn.Cms.Infrastructure.PublishedCache;
using Sbn.Cms.Tests.Common.TestHelpers.Stubs;
using Sbn.Cms.Tests.Integration.Implementations;
using Sbn.Extensions;

namespace Sbn.Cms.Tests.Integration.DependencyInjection
{
    /// <summary>
    /// This is used to replace certain services that are normally registered from our Core / Infrastructure that
    /// we do not want active within integration tests
    /// </summary>
    public static class SbnBuilderExtensions
    {
        /// <summary>
        /// Uses/Replaces services with testing services
        /// </summary>
        public static ISbnBuilder AddTestServices(this ISbnBuilder builder, TestHelper testHelper, AppCaches appCaches = null)
        {
            builder.Services.AddUnique(appCaches ?? AppCaches.NoCache);
            builder.Services.AddUnique(Mock.Of<ISbnBootPermissionChecker>());
            builder.Services.AddUnique(testHelper.MainDom);

            builder.Services.AddUnique<ExamineIndexRebuilder, TestBackgroundIndexRebuilder>();
            builder.Services.AddUnique(factory => Mock.Of<IRuntimeMinifier>());

            // we don't want persisted nucache files in tests
            builder.Services.AddTransient(factory => new PublishedSnapshotServiceOptions { IgnoreLocalDb = true });

#if IS_WINDOWS
            // ensure all lucene indexes are using RAM directory (no file system)
            builder.Services.AddUnique<IDirectoryFactory, LuceneRAMDirectoryFactory>();
#endif

            // replace this service so that it can lookup the correct file locations
            builder.Services.AddUnique(GetLocalizedTextService);

            builder.Services.AddUnique<IServerMessenger, NoopServerMessenger>();
            builder.Services.AddUnique<IProfiler, TestProfiler>();

            return builder;
        }

        /// <summary>
        /// Used to register a replacement for <see cref="ILocalizedTextService"/> where the file sources are the ones within the netcore project so
        /// we don't need to copy files
        /// </summary>
        private static ILocalizedTextService GetLocalizedTextService(IServiceProvider factory)
        {
            IOptions<GlobalSettings> globalSettings = factory.GetRequiredService<IOptions<GlobalSettings>>();
            ILoggerFactory loggerFactory = factory.GetRequiredService<ILoggerFactory>();
            AppCaches appCaches = factory.GetRequiredService<AppCaches>();

            var localizedTextService = new LocalizedTextService(
                new Lazy<LocalizedTextServiceFileSources>(() =>
                {
                    // get the src folder
                    var currFolder = new DirectoryInfo(TestContext.CurrentContext.TestDirectory);
                    while (!currFolder.Name.Equals("src", StringComparison.InvariantCultureIgnoreCase))
                    {
                        currFolder = currFolder.Parent;
                    }

                    DirectoryInfo uiProject = currFolder.GetDirectories("Sbn.Web.UI", SearchOption.TopDirectoryOnly).First();
                    var mainLangFolder = new DirectoryInfo(Path.Combine(uiProject.FullName, globalSettings.Value.SbnPath.TrimStart("~/"), "config", "lang"));

                    return new LocalizedTextServiceFileSources(
                        loggerFactory.CreateLogger<LocalizedTextServiceFileSources>(),
                        appCaches,
                        mainLangFolder);
                }),
                loggerFactory.CreateLogger<LocalizedTextService>());

            return localizedTextService;
        }

        // replace the default so there is no background index rebuilder
        private class TestBackgroundIndexRebuilder : ExamineIndexRebuilder
        {
            public TestBackgroundIndexRebuilder(IMainDom mainDom, IRuntimeState runtimeState, ILogger<ExamineIndexRebuilder> logger, IExamineManager examineManager, IEnumerable<IIndexPopulator> populators, IBackgroundTaskQueue backgroundTaskQueue) : base(mainDom, runtimeState, logger, examineManager, populators, backgroundTaskQueue)
            {
            }

            public override void RebuildIndex(string indexName, TimeSpan? delay = null, bool useBackgroundThread = true)
            {
                // noop
            }

            public override void RebuildIndexes(bool onlyEmptyIndexes, TimeSpan? delay = null, bool useBackgroundThread = true)
            {
                // noop
            }
        }

        private class NoopServerMessenger : IServerMessenger
        {
            public NoopServerMessenger()
            {
            }

            public void QueueRefresh<TPayload>(ICacheRefresher refresher, TPayload[] payload)
            {
            }

            public void QueueRefresh<T>(ICacheRefresher refresher, Func<T, int> getNumericId, params T[] instances)
            {
            }

            public void QueueRefresh<T>(ICacheRefresher refresher, Func<T, Guid> getGuidId, params T[] instances)
            {
            }

            public void QueueRemove<T>(ICacheRefresher refresher, Func<T, int> getNumericId, params T[] instances)
            {
            }

            public void QueueRemove(ICacheRefresher refresher, params int[] numericIds)
            {
            }

            public void QueueRefresh(ICacheRefresher refresher, params int[] numericIds)
            {
            }

            public void QueueRefresh(ICacheRefresher refresher, params Guid[] guidIds)
            {
            }

            public void QueueRefreshAll(ICacheRefresher refresher)
            {
            }

            public void Sync() { }

            public void SendMessages() { }
        }
    }
}
