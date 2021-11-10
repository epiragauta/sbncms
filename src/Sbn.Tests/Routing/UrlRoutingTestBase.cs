using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Tests.Common.Testing;
using Sbn.Extensions;
using Sbn.Tests.TestHelpers;
using Sbn.Tests.Testing;

namespace Sbn.Tests.Routing
{
    [TestFixture]
    [SbnTest(Database = SbnTestOptions.Database.NewSchemaPerFixture)]
    public abstract class UrlRoutingTestBase : BaseWebTest
    {
        /// <summary>
        /// Sets up the mock domain service
        /// </summary>
        /// <param name="allDomains"></param>
        protected IDomainService SetupDomainServiceMock(IEnumerable<IDomain> allDomains)
        {
            var domainService = Mock.Get(ServiceContext.DomainService);
            //setup mock domain service
            domainService.Setup(service => service.GetAll(It.IsAny<bool>()))
                .Returns((bool incWildcards) => incWildcards ? allDomains : allDomains.Where(d => d.IsWildcard == false));
            domainService.Setup(service => service.GetAssignedDomains(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns((int id, bool incWildcards) => allDomains.Where(d => d.RootContentId == id && (incWildcards || d.IsWildcard == false)));
            return domainService.Object;
        }

        protected override void Compose()
        {
            base.Compose();

            Builder.Services.AddUnique(GetServiceContext());
        }

        protected ServiceContext GetServiceContext()
        {
            // get the mocked service context to get the mocked domain service
            var serviceContext = TestObjects.GetServiceContextMock(Factory);

            //setup mock domain service
            var domainService = Mock.Get(serviceContext.DomainService);
            domainService.Setup(service => service.GetAll(It.IsAny<bool>()))
                .Returns((bool incWildcards) => new[]
                {
                    new SbnDomain("domain1.com/"){Id = 1, LanguageId = LangDeId, RootContentId = 1001, LanguageIsoCode = "de-DE"},
                    new SbnDomain("domain1.com/en"){Id = 1, LanguageId = LangEngId, RootContentId = 10011, LanguageIsoCode = "en-US"},
                    new SbnDomain("domain1.com/fr"){Id = 1, LanguageId = LangFrId, RootContentId = 10012, LanguageIsoCode = "fr-FR"}
                });

            return serviceContext;
        }

        public const int LangDeId = 333;
        public const int LangEngId = 334;
        public const int LangFrId = 335;
        public const int LangCzId = 336;
        public const int LangNlId = 337;
        public const int LangDkId = 338;
    }
}
