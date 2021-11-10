using System;
using System.Data;
using Examine.Lucene.Providers;
using Examine.Search;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NPoco;
using NUnit.Framework;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Persistence.Querying;
using Sbn.Cms.Core.Scoping;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Infrastructure.Examine;
using Sbn.Cms.Infrastructure.Persistence;
using Sbn.Cms.Infrastructure.Persistence.SqlSyntax;
using Sbn.Cms.Tests.Integration.Testing;

namespace Sbn.Cms.Tests.Integration.Sbn.Examine.Lucene.SbnExamine
{
    [TestFixture]
    public abstract class ExamineBaseTest : SbnIntegrationTest
    {
        protected IndexInitializer IndexInitializer => Services.GetRequiredService<IndexInitializer>();

        protected IHostingEnvironment HostingEnvironment => Services.GetRequiredService<IHostingEnvironment>();

        protected IRuntimeState RunningRuntimeState { get; } = Mock.Of<IRuntimeState>(x => x.Level == RuntimeLevel.Run);

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddSingleton<IndexInitializer>();
        }

        /// <summary>
        /// Used to create and manage a testable index
        /// </summary>
        /// <param name="publishedValuesOnly"></param>
        /// <param name="index"></param>
        /// <param name="contentRebuilder"></param>
        /// <param name="contentValueSetBuilder"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        protected IDisposable GetSynchronousContentIndex(
            bool publishedValuesOnly,
            out SbnContentIndex index,
            out ContentIndexPopulator contentRebuilder,
            out ContentValueSetBuilder contentValueSetBuilder,
            int? parentId = null,
            IContentService contentService = null)
        {
            contentValueSetBuilder = IndexInitializer.GetContentValueSetBuilder(publishedValuesOnly);

            ISqlContext sqlContext = Mock.Of<ISqlContext>(x => x.Query<IContent>() == Mock.Of<IQuery<IContent>>());
            ISbnDatabaseFactory dbFactory = Mock.Of<ISbnDatabaseFactory>(x => x.SqlContext == sqlContext);

            if (contentService == null)
            {
                contentService = IndexInitializer.GetMockContentService();
            }

            contentRebuilder = IndexInitializer.GetContentIndexRebuilder(contentService, publishedValuesOnly, dbFactory);

            var luceneDir = new RandomIdRAMDirectory();

            ContentValueSetValidator validator;

            // if only published values then we'll change the validator for tests to
            // ensure we don't support protected nodes and that we
            // mock the public access service for the special protected node.
            if (publishedValuesOnly)
            {
                var publicAccessServiceMock = new Mock<IPublicAccessService>();
                publicAccessServiceMock.Setup(x => x.IsProtected(It.IsAny<string>()))
                    .Returns((string path) =>
                    {
                        if (path.EndsWith("," + ExamineDemoDataContentService.ProtectedNode))
                        {
                            return Attempt<PublicAccessEntry>.Succeed();
                        }
                        return Attempt<PublicAccessEntry>.Fail();
                    });

                var scopeProviderMock = new Mock<IScopeProvider>();
                scopeProviderMock.Setup(x => x.CreateScope(
                        It.IsAny<IsolationLevel>(),
                        It.IsAny<RepositoryCacheMode>(),
                        It.IsAny<IEventDispatcher>(),
                        It.IsAny<IScopedNotificationPublisher>(),
                        It.IsAny<bool?>(),
                        It.IsAny<bool>(),
                        It.IsAny<bool>()))
                    .Returns(Mock.Of<IScope>);

                validator = new ContentValueSetValidator(
                    publishedValuesOnly,
                    false,
                    publicAccessServiceMock.Object,
                    scopeProviderMock.Object,
                    parentId);
            }
            else
            {
                validator = new ContentValueSetValidator(publishedValuesOnly, parentId);
            }

            index = IndexInitializer.GetSbnIndexer(
                HostingEnvironment,
                RunningRuntimeState,
                luceneDir,
                validator: validator);

            IDisposable syncMode = index.WithThreadingMode(IndexThreadingMode.Synchronous);

            return new DisposableWrapper(syncMode, index, luceneDir);
        }

        private class DisposableWrapper : IDisposable
        {
            private readonly IDisposable[] _disposables;

            public DisposableWrapper(params IDisposable[] disposables) => _disposables = disposables;

            public void Dispose()
            {
                foreach (IDisposable d in _disposables)
                {
                    d.Dispose();
                }
            }
        }
    }
}
