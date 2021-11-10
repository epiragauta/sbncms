using System;
using Microsoft.Extensions.Options;
using Moq;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Infrastructure.Persistence;
using Sbn.Cms.Infrastructure.Persistence.Mappers;
using Sbn.Cms.Persistence.SqlCe;

namespace Sbn.Tests.Persistence.Mappers
{
    public class MapperTestBase
    {
        protected Lazy<ISqlContext> MockSqlContext()
        {
            var sqlContext = Mock.Of<ISqlContext>();
            var syntax = new SqlCeSyntaxProvider(Options.Create(new GlobalSettings()));
            Mock.Get(sqlContext).Setup(x => x.SqlSyntax).Returns(syntax);
            return new Lazy<ISqlContext>(() => sqlContext);
        }

        protected MapperConfigurationStore CreateMaps()
            => new MapperConfigurationStore();
    }
}
