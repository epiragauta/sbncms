using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using NPoco;
using NUnit.Framework;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Composing;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.DependencyInjection;
using Sbn.Cms.Core.Logging;
using Sbn.Cms.Infrastructure.Persistence;
using Sbn.Cms.Infrastructure.Persistence.Mappers;
using Sbn.Cms.Persistence.SqlCe;
using Sbn.Extensions;
using Sbn.Web;
using Sbn.Web.Composing;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Tests.TestHelpers
{
    [TestFixture]
    public abstract class BaseUsingSqlCeSyntax
    {
        protected IMapperCollection Mappers { get; private set; }

        protected ISqlContext SqlContext { get; private set; }

        internal TestObjects TestObjects = new TestObjects();

        protected Sql<ISqlContext> Sql()
        {
            return NPoco.Sql.BuilderFor(SqlContext);
        }

        [SetUp]
        public virtual void Initialize()
        {
            var services = TestHelper.GetRegister();

            var ioHelper = TestHelper.IOHelper;
            var logger = new ProfilingLogger(Mock.Of<ILogger<ProfilingLogger>>(), Mock.Of<IProfiler>());
            var typeFinder = TestHelper.GetTypeFinder();
            var typeLoader = new TypeLoader(typeFinder, NoAppCache.Instance,
                new DirectoryInfo(ioHelper.MapPath(Constants.SystemDirectories.TempData)),
                Mock.Of<ILogger<TypeLoader>>(),
                logger,
                false);

            var composition = new SbnBuilder(services, Mock.Of<IConfiguration>(), TestHelper.GetMockedTypeLoader());


            services.AddUnique<ILogger>(_ => Mock.Of<ILogger>());
            services.AddUnique<ILoggerFactory>(_ => NullLoggerFactory.Instance);
            services.AddUnique<IProfiler>(_ => Mock.Of<IProfiler>());
            services.AddUnique(typeLoader);

            composition.WithCollectionBuilder<MapperCollectionBuilder>()
                .AddCoreMappers();

            services.AddUnique<ISqlContext>(_ => SqlContext);

            var factory = Current.Factory = TestHelper.CreateServiceProvider(composition);

            var pocoMappers = new NPoco.MapperCollection { new PocoMapper() };
            var pocoDataFactory = new FluentPocoDataFactory((type, iPocoDataFactory) => new PocoDataBuilder(type, pocoMappers).Init());
            var sqlSyntax = new SqlCeSyntaxProvider(Options.Create(new GlobalSettings()));
            SqlContext = new SqlContext(sqlSyntax, DatabaseType.SQLCe, pocoDataFactory, new Lazy<IMapperCollection>(() => factory.GetRequiredService<IMapperCollection>()));
            Mappers = factory.GetRequiredService<IMapperCollection>();

            SetUp();
        }

        public virtual void SetUp()
        {}
    }
}
