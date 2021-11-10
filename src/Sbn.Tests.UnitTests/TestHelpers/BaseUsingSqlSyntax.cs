// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using NPoco;
using NUnit.Framework;
using Sbn.Cms.Core.Composing;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.DependencyInjection;
using Sbn.Cms.Infrastructure.Persistence;
using Sbn.Cms.Infrastructure.Persistence.Mappers;
using Sbn.Cms.Infrastructure.Persistence.SqlSyntax;
using Sbn.Extensions;

namespace Sbn.Cms.Tests.UnitTests.TestHelpers
{
    [TestFixture]
    public abstract class BaseUsingSqlSyntax
    {
        protected IMapperCollection Mappers { get; private set; }

        protected ISqlContext SqlContext { get; private set; }

        protected Sql<ISqlContext> Sql() => NPoco.Sql.BuilderFor(SqlContext);

        [SetUp]
        public virtual void Setup()
        {
            IServiceCollection container = TestHelper.GetServiceCollection();
            TypeLoader typeLoader = TestHelper.GetMockedTypeLoader();

            var composition = new SbnBuilder(container, Mock.Of<IConfiguration>(), TestHelper.GetMockedTypeLoader());

            composition.WithCollectionBuilder<MapperCollectionBuilder>()
                .AddCoreMappers();

            composition.Services.AddUnique(_ => SqlContext);

            IServiceProvider factory = composition.CreateServiceProvider();
            var pocoMappers = new NPoco.MapperCollection
            {
                new NullableDateMapper()
            };
            var pocoDataFactory = new FluentPocoDataFactory((type, iPocoDataFactory) => new PocoDataBuilder(type, pocoMappers).Init());
            var sqlSyntax = new SqlServerSyntaxProvider(Options.Create(new GlobalSettings()));
            SqlContext = new SqlContext(sqlSyntax, DatabaseType.SqlServer2012, pocoDataFactory, factory.GetRequiredService<IMapperCollection>());
            Mappers = factory.GetRequiredService<IMapperCollection>();
        }
    }
}
